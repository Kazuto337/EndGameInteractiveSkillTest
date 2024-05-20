using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BulletBehavior : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float speed;
    bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move(); 
        }
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
        canMove = false;
        Debug.Log(name + "Explode");
        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        canMove = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Explode();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            Explode();
        }
    }
}
