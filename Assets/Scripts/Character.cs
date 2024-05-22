using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float healthPoints;
    [SerializeField] protected float maxHealthPoints;

    public void ReceiveDamage(float damageValue)
    {
        healthPoints -= damageValue;
    }
}
