using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{


    [SerializeField] private Weapon originalWeapon;
    private Weapon holdingThisWeaponScript;

    private void Start()
    {
        DetachWeapon();
    }

    
    public void SwitchWeapon(Weapon weaponScript)
    {
        this.holdingThisWeaponScript.enabled = false;
        this.holdingThisWeaponScript = weaponScript;
        holdingThisWeaponScript.enabled = true;

        print("Switched");
        
    }

    public void DetachWeapon()
    {
        if (holdingThisWeaponScript != null)
        {
            holdingThisWeaponScript.enabled = false;
        }
        
        this.holdingThisWeaponScript = originalWeapon;
        holdingThisWeaponScript.enabled = true;

        print("Detached");
    }




}
