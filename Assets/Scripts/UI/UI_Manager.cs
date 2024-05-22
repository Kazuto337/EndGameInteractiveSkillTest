using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas canvasMobile , canvasPC;
    [SerializeField] AmmoUI ammoUI;
    [SerializeField] InventorySlot slotMedkit, slotAmmo, slotKeys;

    public AmmoUI AmmoUI { get => ammoUI; }
    public InventorySlot SlotMedkit { get => slotMedkit; }
    public InventorySlot SlotAmmo { get => slotAmmo;}
    public InventorySlot SlotKeys { get => slotKeys;}

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        canvasMobile.gameObject.SetActive(true);
        canvasPC.gameObject.SetActive(false);
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        canvasMobile.gameObject.SetActive(false);
        canvasPC.gameObject.SetActive(true);
#else
        canvasMobile.gameObject.SetActive(false);
        canvasPC.gameObject.SetActive(true);
#endif
    }
}
