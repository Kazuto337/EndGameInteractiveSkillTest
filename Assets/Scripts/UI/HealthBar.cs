using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthBarSlider;

    private float maxHP;
    private float currentHP;

    [SerializeField] private Transform target;
    private Character targetCharacter;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        healthBarSlider.minValue = 0;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset; 
        }

        healthBarSlider.value = targetCharacter.HealthPoints;
    }

    public void SetValues(float maxHP , float currentHP)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;

        healthBarSlider.maxValue = maxHP;
        healthBarSlider.value = currentHP;
    }

    private void OnEnable()
    {
        healthBarSlider = GetComponent<Slider>();
        healthBarSlider.maxValue = maxHP;
    }
    public void FollowTransform(Transform _transform)
    {
        target = _transform;
        targetCharacter = _transform.GetComponent<Character>();
    }
}
