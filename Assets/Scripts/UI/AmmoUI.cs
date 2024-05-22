using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] TMP_Text ammoTxt;

    public void ModifyAmmoText(int ammoOnStack , int ammo)
    {
        ammoTxt.text = ammoOnStack + "/" + ammo;
    }
}
