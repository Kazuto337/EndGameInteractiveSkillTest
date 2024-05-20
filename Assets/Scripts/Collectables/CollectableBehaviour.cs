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
    [SerializeField] float amplitude , speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] CollectableType type;

    public CollectableType Type { get => type;}

    void Update()
    {
        gameObject.transform.position = new Vector3(transform.position.x, 0.5f + Mathf.Sin(Time.time * speed) * amplitude , transform.position.z);
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
