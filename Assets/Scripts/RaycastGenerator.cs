using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGenerator : MonoBehaviour
{
    public static RaycastGenerator Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else Instance = this;
    }
    public Vector3 GenerateRaycast(Vector3 origin, Vector3 destination, float lenght , string desireTag , LayerMask ignoreLayer)
    {
        int layer = ~ignoreLayer.value;

        RaycastHit hit;
        Debug.DrawRay(origin, destination, Color.red);
        if (Physics.Raycast(origin , destination , out hit , lenght , layer))
        {
            if (hit.collider.CompareTag(desireTag))
            {
                return hit.collider.transform.position;
            }
        }

        return new Vector3();
    }
}
