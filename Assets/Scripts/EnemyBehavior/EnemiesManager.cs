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
        GameObject newHealthBar = Instantiate(healthBarPrefab, newEnemy.transform);
        newHealthBar.GetComponent<HealthBar>().SetValues(newEnemy.GetComponent<Character>().MaxHealthPoints , newEnemy.GetComponent<Character>().HealthPoints);

        enemies.Add(newEnemy.GetComponent<EnemyBehavior>());

    }
}
