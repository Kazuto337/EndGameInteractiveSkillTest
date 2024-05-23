using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesManager : MonoBehaviour
{
    List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Canvas enemiesStatsCanvas;

    int enemiesAlive;

    public UnityEvent<int> EnemyEliminated;

    public int EnemiesAlive { get => enemiesAlive;}

    private void Update()
    {
        enemiesAlive = enemies.Count;
    }
    public void GenerateEnemy(Vector3 spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, enemyPrefab.transform.rotation);
        GameObject newHealthBar = Instantiate(healthBarPrefab, enemiesStatsCanvas.transform);

        EnemyBehavior enemyComponent = newEnemy.GetComponent<EnemyBehavior>();
        HealthBar healthBarComponent = newHealthBar.GetComponent<HealthBar>();
        healthBarComponent.SetValues(enemyComponent.MaxHealthPoints, enemyComponent.HealthPoints);
        healthBarComponent.FollowTransform(newEnemy.transform);

        enemyComponent.OnCharacterDead.AddListener(OnEnemyDead);
        enemies.Add(newEnemy.GetComponent<EnemyBehavior>());
    }

    public void OnEnemyDead(EnemyBehavior deathEnemy)
    {
        enemies.Remove(deathEnemy);
        Destroy(deathEnemy.gameObject );

        EnemyEliminated.Invoke(enemiesAlive);
    }

    private void OnDisable()
    {
        EnemyEliminated.RemoveAllListeners();
    }
}
