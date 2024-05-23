using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimSystem : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float aimSpeed = 5f;
    [SerializeField] private float returnSpeed = 2f;

    [SerializeField] PlayerBehaviour playerBehaviour;

    private Transform closestEnemy;
    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        DetectEnemies();
        if (closestEnemy != null)
        {
            AimAtClosestEnemy();
        }
    }

    private void DetectEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        if (enemiesInRange.Length == 0)
        {
            ReturnToOriginalRotation();
            return;
        }

        foreach (Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
    }

    private void AimAtClosestEnemy()
    {
        Vector3 directionToEnemy = closestEnemy.position - transform.position;
        directionToEnemy.y = 0; // Ignore the y-axis to rotate only around the Y axis
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void ReturnToOriginalRotation()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * returnSpeed);
        Quaternion lookRotation = Quaternion.LookRotation(playerBehaviour.moveValue);
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}