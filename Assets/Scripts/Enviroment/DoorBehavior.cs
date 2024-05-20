using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
