using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float healthPoints;
    [SerializeField] private float maxHealthPoints;

    [Header("Animation Properties")]
    private bool isMoving;
    private bool isShooting;
    private Animator animator;

    [Header("Movement Attributes")]
    [SerializeField] int detectionRadius;
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
    }

    private void PlayerDetector()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, detectionRadius, transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.LogWarning("Player detected by " + gameObject.name);
                Move(hit.point);
                return;
            }
        }

        StartCoroutine(SetRandomDestination());
    }

    // Gizmo for scene view visualization
    void OnDrawGizmos()
    {
        // Visualization in the Scene view for debugging
        Debug.DrawLine(transform.position, transform.position + transform.forward * detectionRadius, Color.red);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * detectionRadius, detectionRadius);
    }

    private IEnumerator SetRandomDestination()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
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

}
