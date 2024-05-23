using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] GameObject player , healthBar;
    [SerializeField] EnemiesManager enemies;

    [SerializeField] GameObject medKitPrefab, keyPrefab, ammoPrefab;

    [SerializeField] Vector3 mapSize;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] ObstacleDetector obstacleDetector;

    int enemiesLeft;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        enemies.EnemyEliminated.AddListener(Win);
    }

    private void Start()
    {
        SpawnPlayer();       

        SpawnEnemies();
        SpawnCollectables();

    }

    private void Update()
    {
        enemiesLeft = enemies.EnemiesAlive;
        ui_manager.UpdateEnemies(enemiesLeft);
    }

    private void SpawnPlayer()
    {
        float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
        float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

        Vector3 playerPosition = new Vector3(xValue, mapSize.y, zValue);

        player = Instantiate(player, playerPosition, player.transform.rotation);        

        virtualCamera.LookAt = player.transform;
        virtualCamera.Follow = player.transform;

        PlayerBehaviour playerComponent = player.GetComponent<PlayerBehaviour>();
        obstacleDetector.SetPlayer(player.transform);

        playerComponent.OnAmmoChanged.AddListener(ui_manager.AmmoUI.ModifyAmmoText);
        playerComponent.OnKeyFound.AddListener(ui_manager.SlotKeys.AddElement);
        playerComponent.OnMedKitFound.AddListener(ui_manager.SlotMedkit.AddElement);

        playerComponent.OnMedKitUsed.AddListener(ui_manager.SlotMedkit.RemoveElement);
        playerComponent.OnKeyUsed.AddListener(ui_manager.SlotKeys.RemoveElement);

        playerComponent.OnCharacterDead.AddListener(Loose);

        GameObject playerHealthBar = Instantiate(healthBar, ui_manager.worldSpaceCanvas.transform);
        playerComponent.HealthBar = playerHealthBar.GetComponent<HealthBar>();
        playerHealthBar.GetComponent<HealthBar>().FollowTransform(player.transform);

    }

    private void SpawnEnemies()
    {
        int randomEnemies = Random.Range(4, 6);

        float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
        float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

        Vector3 enemyPosition = new Vector3(xValue, mapSize.y, zValue);

        for (int i = 0; i < randomEnemies; i++)
        {
            xValue = Random.Range(-1 * mapSize.x, mapSize.x);
            zValue = Random.Range(-1 * mapSize.z, mapSize.z);

            enemyPosition = new Vector3(xValue, mapSize.y, zValue);
            enemies.GenerateEnemy(enemyPosition);
        }
    }

    private void SpawnCollectables()
    {
        int index = Random.Range(4 , 6);
        

        for (int i = 0; i < index; i++)
        {
            float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
            float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

            Vector3 collectablePos = new Vector3(xValue, mapSize.y, zValue);
            Instantiate(ammoPrefab, collectablePos, ammoPrefab.transform.rotation);
        }
        
        index = Random.Range(4 , 8);        

        for (int i = 0; i < index; i++)
        {
            float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
            float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

            Vector3 collectablePos = new Vector3(xValue, mapSize.y, zValue);
            Instantiate(medKitPrefab, collectablePos, ammoPrefab.transform.rotation);
        }
        
        index = Random.Range(2 , 4);        

        for (int i = 0; i < index; i++)
        {
            float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
            float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

            Vector3 collectablePos = new Vector3(xValue, mapSize.y, zValue);
            Instantiate(keyPrefab, collectablePos, ammoPrefab.transform.rotation);
        }
    }
    public void Win(int enemiesLeft)
    {
        if (enemiesLeft != 0)
        {
            ui_manager.UpdateEnemies(enemiesLeft);
            return;
        }

        ScenesManager.LoadScene(2);
    }
    public void Loose(PlayerBehaviour player)
    {
        ScenesManager.LoadScene(3);
    }
}
