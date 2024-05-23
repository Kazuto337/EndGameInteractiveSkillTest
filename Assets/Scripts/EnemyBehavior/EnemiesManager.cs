using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject healthBarPrefab;

    [SerializeField] Canvas enemiesStatsCanvas;
    public void GenerateEnemy(Vector3 spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab , spawnPoint , enemyPrefab.transform.rotation);
        GameObject newHealthBar = Instantiate(healthBarPrefab, enemiesStatsCanvas.transform);

        EnemyBehavior enemyComponent = newEnemy.GetComponent<EnemyBehavior>();
        HealthBar healthBarComponent = newHealthBar.GetComponent<HealthBar>();

        healthBarComponent.SetValues(enemyComponent.MaxHealthPoints , enemyComponent.HealthPoints);
        healthBarComponent.FollowTransform(newEnemy.transform);

        enemies.Add(newEnemy.GetComponent<EnemyBehavior>());

    }
}
