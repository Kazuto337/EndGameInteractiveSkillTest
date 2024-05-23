using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] GameObject player;
    [SerializeField] EnemiesManager enemies;

    [SerializeField] Vector3 mapSize;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] ObstacleDetector obstacleDetector;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    private void Start()
    {
        SpawnPlayer();

        virtualCamera.LookAt = player.transform;
        virtualCamera.Follow = player.transform;

        obstacleDetector.SetPlayer(player.transform);

        SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        float xValue = Random.Range(-1 * mapSize.x, mapSize.x);
        float zValue = Random.Range(-1 * mapSize.z, mapSize.z);

        Vector3 playerPosition = new Vector3(xValue, mapSize.y, zValue);

        player = Instantiate(player, playerPosition, player.transform.rotation);

        player.GetComponent<PlayerBehaviour>().OnAmmoChanged.AddListener(ui_manager.AmmoUI.ModifyAmmoText);
        player.GetComponent<PlayerBehaviour>().OnKeyFound.AddListener(ui_manager.SlotKeys.AddElement);
        player.GetComponent<PlayerBehaviour>().OnMedKitFound.AddListener(ui_manager.SlotMedkit.AddElement);

        player.GetComponent<PlayerBehaviour>().OnMedKitUsed.AddListener(ui_manager.SlotMedkit.RemoveElement);
        player.GetComponent<PlayerBehaviour>().OnKeyUsed.AddListener(ui_manager.SlotKeys.RemoveElement);
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
}
