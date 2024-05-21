using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBehavior : MonoBehaviour
{
    [SerializeField] GameObject roof;

    public void TurnOffRoof()
    {
        roof.SetActive(false);
    }
    public void TurnOnRoof()
    {
        roof.SetActive(true);
    }
}
