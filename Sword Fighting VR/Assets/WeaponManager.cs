using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{


    [SerializeField] private Weapon originalWeapon;
    private Weapon weaponScript;

    private void Start()
    {
        DetachWeapon();
    }

    
    public void SwitchWeapon(Weapon weaponScript)
    {
        weaponScript.enabled = false;
        this.weaponScript = weaponScript;
        weaponScript.enabled = true;
        
    }

    public void DetachWeapon()
    {
        if (weaponScript != null)
        {
            weaponScript.enabled = false;
        }
        
        this.weaponScript = originalWeapon;
        weaponScript.enabled = true;
    }




}
