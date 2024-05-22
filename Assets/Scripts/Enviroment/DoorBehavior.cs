using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    public void OpenDoor()
    {
        gameObject.SetActive(false);
        obstacle.SetActive(false);
    }
}
