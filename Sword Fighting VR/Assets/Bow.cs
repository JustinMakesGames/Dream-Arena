using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bow : Weapon
{
    [SerializeField] private Transform bowString;
    public override int GetDamage(Collider collision)
    {
        return base.GetDamage();
    }

    public void Shoot()
    {
        throw new NotImplementedException();
    }
}
