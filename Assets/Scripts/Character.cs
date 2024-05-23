using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float healthPoints;
    [SerializeField] protected float maxHealthPoints;

    public float HealthPoints { get => healthPoints;}
    public float MaxHealthPoints { get => maxHealthPoints;}

    private void OnEnable()
    {
        healthPoints = MaxHealthPoints;
    }
    public void ReceiveDamage(float damageValue)
    {
        healthPoints -= damageValue;
        Debug.LogWarning(gameObject.name + "Received Damage. Current HP = " + HealthPoints);
    }
}
