using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerActions playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private void Awake()
    {
        playerInput = new PlayerActions();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementVector = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 moveValue = new Vector3(movementVector.x, 0, movementVector.y);
        controller.Move(moveValue * Time.deltaTime * playerSpeed);

        if (moveValue != Vector3.zero)
        {
            gameObject.transform.forward = moveValue;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
