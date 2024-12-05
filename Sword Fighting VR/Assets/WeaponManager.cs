using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{


    [SerializeField] private Weapon originalWeapon;
    private Weapon _weaponScript;

    private void Start()
    {
        DetachWeapon();
    }

    
    public void SwitchWeapon(Weapon weaponScript)
    {
        this._weaponScript.isEquipped = false;
        this._weaponScript = weaponScript;
        _weaponScript.isEquipped = true;

        print("Switched");
        
    }

    public void DetachWeapon()
    {
        if (_weaponScript != null)
        {
            _weaponScript.isEquipped = false;
        }
        
        this._weaponScript = originalWeapon;
        _weaponScript.isEquipped = true;

        print("Detached");
    }




}
