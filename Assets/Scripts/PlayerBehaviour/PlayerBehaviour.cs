using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerActions playerInput;
    [Header("Stats")]
    [SerializeField] private float healthPoints;
    [SerializeField] private float maxHealthPoints;

    [Header("Movement Attributes")]
    private CharacterController controller;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Animation Properties")]
    private bool isMoving;
    private bool isShooting;
    private bool isHealing;
    private Animator animator;

    [Header("Inventory Attributes")]
    [SerializeField] WeaponBehavior weapon;
    [SerializeField] CollectableBehaviour[] medKits = new CollectableBehaviour[4];
    [SerializeField] CollectableBehaviour[] keys = new CollectableBehaviour[3];

    private void Awake()
    {
        healthPoints = maxHealthPoints;

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
        if (weapon.Ammo == 0 || weapon.IsLoading)
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
        ReloadWeapon();
        UseMedKit();
    }

    private void OpenDoor(DoorBehavior door)
    {
        if (playerInput.PlayerMain.Interact.ReadValue<float>() < 1)
        {
            return;
        }

        int emptySlots = 0;
        foreach (var item in keys)
        {
            if (item == null)
            {
                emptySlots++;
            }
        }

        if (emptySlots == keys.Length)
        {
            Debug.LogWarning("You don't have a key to open this door!");
            return;
        }

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != null)
            {
                keys[i] = null;
            }
        }
        door.OpenDoor();
    }

    private void ReloadWeapon()
    {
        if (playerInput.PlayerMain.Reload.ReadValue<float>() > 0)
        {
            weapon.ReloadWeapon();
        }
    }

    private void UseMedKit()
    {
        int nullSpaces = 0;
        int kits = 0;


        for (int i = 0; i < medKits.Length; i++)
        {
            if (medKits[i] != null)
            {
                kits++;
            }
            else
            {
                nullSpaces++;
            }
        }
        bool conditionA = nullSpaces == medKits.Length;
        bool conditionB = healthPoints == maxHealthPoints;

        if (conditionA || conditionB || isHealing)
        {
            return;
        }

        if (playerInput.PlayerMain.MedKit.ReadValue<float>() < 1)
        {
            return;
        }

        isHealing = true;
        StartCoroutine(HealingBehavior());
    }

    private IEnumerator HealingBehavior()
    {
        for (int i = 0; i < medKits.Length; i++)
        {
            if (medKits[i] != null)
            {
                medKits[i] = null;
                break;
            }
        }

        float medKitValue = 30f;
        if (medKitValue + healthPoints > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
            yield break;
        }

        healthPoints += medKitValue;

        yield return new WaitForSeconds(1f);

        isHealing = false;
    }

    private void SetAnimationStates()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isShooting", isShooting);
    }

    private void OnAmmoFound()
    {
        Debug.LogWarning("Ammo Found");
        weapon.NewAmmo();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            CollectableBehaviour collectable = other.GetComponent<CollectableBehaviour>();

            switch (collectable.Type)
            {
                case CollectableType.ammo:
                    if (weapon.Ammo == weapon.MaxAmmo)
                    {
                        break;
                    }
                    OnAmmoFound();
                    other.gameObject.SetActive(false);

                    break;

                case CollectableType.medKit:

                    for (int i = 0; i < medKits.Length; i++)
                    {
                        if (medKits[i] == null)
                        {
                            medKits[i] = other.GetComponent<CollectableBehaviour>();
                            other.gameObject.SetActive(false);
                            break;
                        }
                    }
                    break;

                case CollectableType.key:
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (keys[i] == null)
                        {
                            keys[i] = other.GetComponent<CollectableBehaviour>();
                            other.gameObject.SetActive(false);
                            break;
                        }
                    }
                    break;

                default:
                    break;
            }
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DoorInteractable"))
        {
            Debug.LogWarning("Door Found.");
            OpenDoor(other.GetComponent<DoorBehavior>());
        }
    }
}
