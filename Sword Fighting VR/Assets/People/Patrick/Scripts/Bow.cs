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
        Shoot(transform.position, 5, transform.rotation);
    }
    
    public void Shoot(Vector3 arrowPosition, float speed, Quaternion arrowRotation)
    {
        GameObject instantiatedArrow = Instantiate(arrow, arrowPosition, arrowRotation);
        instantiatedArrow.GetComponentInChildren<ArrowPhysics>().speed = speed;
    }
}
