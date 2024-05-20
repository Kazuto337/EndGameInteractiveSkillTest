using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [Header("Weapon Properties")]
    [SerializeField] private int ammo;
    [SerializeField] private int ammoOnStack;
    [SerializeField] private float reloadTime;
    [SerializeField] private float fireRate;
    private int maxAmmo = 200;
    private int stack = 25;
    private bool isLoading , isShooting;

    [Header("Shooting Atrttributes")]
    [SerializeField] private List<BulletBehavior> bulletsPool;
    [SerializeField] private Transform bulletSpawnPoint;

    public int Ammo { get => ammo;}

    private void Start()
    {
        ResetWeapon();
    }
    public void ReloadWeapon()
    {
        if (ammo == 0)
        {
            return;
        }

        isLoading = true;
        StartCoroutine(ReloadBehavior());
    }

    private IEnumerator ReloadBehavior()
    {
        yield return new WaitForSeconds(reloadTime);

        int ammoDifference = stack - ammoOnStack;
        ammo = ammoDifference;

        //If ammo is minor than the stack value, then put the leftover in the charger
        ammoOnStack = ammo > stack ? stack : ammo;
        isLoading = false;
    }

    public void FireWeapon()
    {
        if (isLoading)
        {
            return;
        }

        if (isShooting)
        {
            return;
        }

        if (ammo == 0)
        {
            return;
        }

        StartCoroutine(FireWeaponBehavior());
    }

    private IEnumerator FireWeaponBehavior()
    {
        isShooting = true;
        foreach (BulletBehavior bullet in bulletsPool)
        {
            if (bullet.gameObject.activeInHierarchy == false)
            {
                bullet.GetComponent<Transform>().position = bulletSpawnPoint.position;

                bullet.Fire();
                break;
            }
        }

        ammo --;
        ammoOnStack --;

        if (ammoOnStack == 0)
        {
            ReloadWeapon();
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    /// <summary>
    /// Activate on new ammo find
    /// </summary>
    public void NewAmmo()
    {
        ammo = maxAmmo;
    }

    public void ResetWeapon()
    {
        ammo = maxAmmo;
        ammoOnStack = stack;
    }
}
