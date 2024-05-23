using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthBarSlider;

    private float maxHP;
    private float currentHP;

    public void SetValues(float maxHP , float currentHP)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;

        healthBarSlider.value = currentHP;
    }

    public void UpdateCurrentHP(float newHP)
    {
        currentHP = newHP;
        healthBarSlider.value = newHP;
    }

    private void OnEnable()
    {
        healthBarSlider = GetComponent<Slider>();
        healthBarSlider.maxValue = maxHP;
    }

    private void Update()
    {
        FollowCharacter();
    }

    private void FollowCharacter()
    {

    }
}
