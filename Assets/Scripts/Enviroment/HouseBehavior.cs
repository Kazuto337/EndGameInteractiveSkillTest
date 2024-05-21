using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBehavior : MonoBehaviour
{
    public void TurnOffRoof()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    public void TurnOnRoof()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
