using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class EnemyBehavior : Character
{
    [Header("Animation Properties")]
    private bool isMoving;
    private bool isShooting;
    private Animator animator;

    [Header("Movement Attributes")]
    [SerializeField] int detectionRadius;
    [SerializeField] bool playerFound;
    [SerializeField] LayerMask sphereMask;
    EnemyPathCreation pathCreation;
    NavMeshAgent agent;

    [Header("Inventory")]
    [SerializeField] WeaponBehavior weapon;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        pathCreation = EnemyPathCreation.instance;
    }

    private void Update()
    {
        PlayerDetector();

        if (playerFound)
        {
            isShooting = true;
            Shoot();
        }
        else isShooting = false;

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            isMoving = true;
        }
        else isMoving = false;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isShooting", isShooting);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private void PlayerDetector()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, sphereMask);
        Vector3 player = Vector3.zero;

        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                Vector3 directionToPlayer = (item.transform.position - transform.position);

                player = RaycastGenerator.Instance.GenerateRaycast(transform.position, directionToPlayer , Mathf.Infinity, "Player");
                break;
            }
        }

        if (player != Vector3.zero)
        {
            transform.LookAt(player);
            Move(player);
            playerFound = true;
            return;
        }

        playerFound = false;
        StartCoroutine(SetRandomDestination());
    }
    private IEnumerator SetRandomDestination()
    {
        if (agent.remainingDistance > agent.stoppingDistance || playerFound)
        {
            yield break;
        }

        Move(pathCreation.GetNextPosition());
        yield return null;
    }

    private void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void Shoot()
    {
        if (weapon.Ammo == 0 || weapon.IsLoading)
        {
            isShooting = false;
            return;
        }

        isShooting = true;
        weapon.FireWeapon();
        return;
    }
}
