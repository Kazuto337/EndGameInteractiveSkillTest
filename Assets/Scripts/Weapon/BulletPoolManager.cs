using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] List<BulletBehavior> bulletPool;
    [SerializeField] Vector3 bulletsSpawn;

    public List<BulletBehavior> BulletPool { get => bulletPool;}

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        for (int i = 0; i < 500; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab , bulletsSpawn , bulletPrefab.transform.rotation);
            bulletPool.Add(newBullet.GetComponent<BulletBehavior>());
        }
    }
}
