using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathCreation : MonoBehaviour
{
    public static EnemyPathCreation instance;

    [SerializeField] Vector2 mapSize;
    Camera _camera;

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
        _camera = Camera.main;
    }

    public Vector3 GetNextPosition()
    {
        float posX = Random.Range(mapSize.x * -1, mapSize.x);
        float posY = Random.Range(mapSize.y * -1, mapSize.y);

        return new Vector3(posX , 0 , posY);
    }
}
