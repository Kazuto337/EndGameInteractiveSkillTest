using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] CustomCanvas canvasMobile, canvasPC;
    AmmoUI ammoUI;
    InventorySlot slotMedkit, slotAmmo, slotKeys;

    public AmmoUI AmmoUI { get => ammoUI; }
    public InventorySlot SlotMedkit { get => slotMedkit; }
    public InventorySlot SlotAmmo { get => slotAmmo; }
    public InventorySlot SlotKeys { get => slotKeys; }

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        canvasMobile.gameObject.SetActive(true);
        canvasPC.gameObject.SetActive(false);

        ammoUI = canvasMobile.ammoUI;
        slotAmmo = canvasMobile.slotAmmo;
        slotKeys = canvasMobile.slotKeys;
        slotMedkit = canvasMobile.slotMedkit;

#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        canvasMobile.gameObject.SetActive(false);
        canvasPC.gameObject.SetActive(true);

        ammoUI = canvasPC.ammoUI;
        slotAmmo = canvasPC.slotAmmo;
        slotKeys = canvasPC.slotKeys;
        slotMedkit = canvasPC.slotMedkit;
#else
        canvasMobile.gameObject.SetActive(false);
        canvasPC.gameObject.SetActive(true);

        ammoUI = canvasPC.ammoUI;
        slotAmmo = canvasPC.slotAmmo;
        slotKeys = canvasPC.slotKeys;
        slotMedkit = canvasPC.slotMedkit;
#endif
    }
}
