using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    ammo = 0,
    medKit,
    key
}
public class CollectableBehaviour : MonoBehaviour
{
    float amplitude = 0.05f, speed = 3f;
    float rotationSpeed = 45;
    [SerializeField] CollectableType type;

    public CollectableType Type { get => type;}

    void Update()
    {
        gameObject.transform.position = new Vector3(transform.position.x, 0.5f + Mathf.Sin(Time.time * speed) * amplitude , transform.position.z);
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
