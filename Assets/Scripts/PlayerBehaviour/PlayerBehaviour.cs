using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : Character
{
    [SerializeField] HealthBar healthBar;
    private PlayerActions playerInput;
    private bool isHealing;

    [Header("Movement Attributes")]
    private CharacterController controller;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    public Vector3 moveValue;

    [Header("Animation Properties")]
    private bool isMoving;
    private bool isShooting;
    [SerializeField] private Animator animator;

    [Header("Inventory Attributes")]
    [SerializeField] WeaponBehavior weapon;
    [SerializeField] CollectableBehaviour[] medKits = new CollectableBehaviour[4];
    [SerializeField] CollectableBehaviour[] keys = new CollectableBehaviour[3];

    [Header("UI Events")]
    public UnityEvent<float> OnHPChanged;
    public UnityEvent<int, int> OnAmmoChanged;
    public UnityEvent OnMedKitFound;
    public UnityEvent OnMedKitUsed;
    public UnityEvent OnKeyFound;
    public UnityEvent OnKeyUsed;
    public UnityEvent<PlayerBehaviour> OnCharacterDead;

    public HealthBar HealthBar { get => healthBar; set => healthBar = value; }

    private void Awake()
    {
        playerInput = new PlayerActions();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Start()
    {
        OnAmmoChanged.Invoke(weapon.AmmoOnStack, weapon.Ammo);
    }
    private void Update()
    {
        Move();
        Shoot();
        SetAnimationStates();
        ReloadWeapon();
        UseMedKit();
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.SetValues(maxHealthPoints, healthPoints);
    }
    private void Move()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }

        Vector2 movementVector = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        moveValue = new Vector3(movementVector.x, 0, movementVector.y);
        isMoving = moveValue != Vector3.zero;

        controller.Move(moveValue * Time.deltaTime * playerSpeed);

        if (moveValue != Vector3.zero)
        {
            gameObject.transform.forward = moveValue;
        }

        verticalVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

    }

    public void UpdateAmmo()
    {
        OnAmmoChanged.Invoke(weapon.AmmoOnStack, weapon.Ammo);
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
    private void ReloadWeapon()
    {
        if (playerInput.PlayerMain.Reload.ReadValue<float>() > 0)
        {
            weapon.ReloadWeapon();
        }
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
        OnKeyUsed.Invoke();
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
        bool conditionB = HealthPoints == MaxHealthPoints;

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
        OnMedKitUsed.Invoke();
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
        if (medKitValue + HealthPoints > MaxHealthPoints)
        {
            healthPoints = MaxHealthPoints;
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

        OnAmmoChanged.Invoke(weapon.AmmoOnStack, weapon.Ammo);
    }

    private void OnDisable()
    {
        playerInput.Disable();

        OnHPChanged.RemoveAllListeners();
        OnAmmoChanged.RemoveAllListeners();
        OnMedKitFound.RemoveAllListeners();
        OnMedKitUsed.RemoveAllListeners();
        OnKeyFound.RemoveAllListeners();
        OnKeyUsed.RemoveAllListeners();
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

                            OnMedKitFound.Invoke();
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
                            OnKeyFound.Invoke();
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
    public override void Die()
    {
        base.Die();
        OnCharacterDead.Invoke(this);
    }

    private void OnDestroy()
    {
        OnCharacterDead.RemoveAllListeners();
    }
}
