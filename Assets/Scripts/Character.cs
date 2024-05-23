using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float healthPoints;
    [SerializeField] protected float maxHealthPoints;
        
    public float HealthPoints { get => healthPoints;}
    public float MaxHealthPoints { get => maxHealthPoints;}

    private void Start()
    {
        healthPoints = MaxHealthPoints;
    }

    private void OnEnable()
    {
        healthPoints = MaxHealthPoints;
    }
    public void ReceiveDamage(float damageValue)
    {
        healthPoints -= damageValue;
        Debug.LogWarning(gameObject.name + "Received Damage. Current HP = " + HealthPoints);

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

}
