using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BulletBehavior : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float speed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!gameObject.activeInHierarchy) return;
        Vector3 movement = transform.forward * speed;

        characterController.Move(movement);
    }

    public void Fire()
    {
        gameObject.SetActive(true);
    }

    private void Explode()
    {
        Debug.Log(name + "Explode");
        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Explode();
    }
}
