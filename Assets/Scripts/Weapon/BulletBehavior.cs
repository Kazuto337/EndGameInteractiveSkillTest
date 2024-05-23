using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] GameObject bulletMesh;
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
        bulletMesh.SetActive(false);
        _particleSystem.gameObject.SetActive(true );
        _particleSystem.Play();
        yield return new WaitForSeconds(_particleSystem.main.duration);
        gameObject.SetActive(false);
        _particleSystem.gameObject.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = true;
        canMove = true;
        bulletMesh.SetActive(true);
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
            Debug.Log("Hit " + other.name + " current HP = " + other.GetComponent<Character>().HealthPoints);
            Explode();
            other.GetComponent<Character>().ReceiveDamage(bulletDamage);
        }
    }
}
