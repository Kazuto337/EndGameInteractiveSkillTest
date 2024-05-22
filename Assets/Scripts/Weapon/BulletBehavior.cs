using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float bulletDamage;
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
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = true;
        canMove = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Explode();
        if (hit.collider.CompareTag("Player"))
        {
            hit.collider.gameObject.GetComponent<Character>().ReceiveDamage(bulletDamage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            Explode();
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit " + other.name);
            Explode();
            other.GetComponent<Character>().ReceiveDamage(bulletDamage);
        }
    }
}
