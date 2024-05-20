using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerActions playerInput;

    [Header("Movement Attributes")]
    private CharacterController controller;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Animation Properties")]
    private bool isMoving;
    private bool isShooting;
    private Animator animator;

    [Header("Inventory Attributes")]
    [SerializeField] WeaponBehavior weapon;

    private void Awake()
    {
        playerInput = new PlayerActions();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Move()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }

        Vector2 movementVector = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 moveValue = new Vector3(movementVector.x, 0, movementVector.y);

        isMoving = moveValue != Vector3.zero;

        controller.Move(moveValue * Time.deltaTime * playerSpeed);

        if (moveValue != Vector3.zero)
        {
            gameObject.transform.forward = moveValue;
        }

        verticalVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void Shoot()
    {
        if(weapon.Ammo == 0 || weapon.IsLoading)
        {
            isShooting = false;
            return;
        }

        bool isButtonPress = playerInput.PlayerMain.Fire.ReadValue<float>() > 0;

        if (isButtonPress)
        {
            isShooting = true;
            weapon.FireWeapon();
            return;
        }

        isShooting = false;
    }

    private void Update()
    {
        Move();
        Shoot();
        SetAnimationStates();
    }

    private void SetAnimationStates()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isShooting", isShooting);
    }
    
    private void OnAmmoFound()
    {
        weapon.NewAmmo();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            OnAmmoFound();
            other.gameObject.SetActive(false);
        }
    }
}
