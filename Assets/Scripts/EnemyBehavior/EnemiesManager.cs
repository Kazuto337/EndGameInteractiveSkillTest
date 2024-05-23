using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<EnemyBehavior> enemies;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Canvas enemiesStatsCanvas;

    public void GenerateEnemy(Vector3 spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab , spawnPoint , enemyPrefab.transform.rotation);
        enemies.Add(newEnemy.GetComponent<EnemyBehavior>());
    }
}
