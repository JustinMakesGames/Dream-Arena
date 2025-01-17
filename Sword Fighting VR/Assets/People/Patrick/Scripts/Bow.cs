using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bow : Weapon
{
    [SerializeField] private GameObject arrow;
    
    [ContextMenu("Shoot")]
    public void TestShoot()
    {
        Shoot(transform.position);
    }
    
    public void Shoot(Vector3 arrowPosition)
    {
        if (Physics.Raycast(arrowPosition, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject go = hit.collider.gameObject;
            if (go.TryGetComponent(out PlayerHealth p))
            {
                p.TakeDamage(baseDamage);
            }
        }
    }
}
