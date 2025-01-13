using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;
    private int _damage;

    private void Start()
    {
        _damage = weapon.damage;
    }

    public virtual int Damage()
    {
        return Random.Range(_damage - 5, _damage + 5);
    }
}
