using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bow : Weapon
{
    [SerializeField] private Transform bowString;
    [SerializeField] private GameObject arrow;
    
    public void Shoot(Vector3 arrowPosition, float speed)
    {
        GameObject instantiatedArrow = Instantiate(arrow, arrowPosition, Quaternion.identity);
        instantiatedArrow.GetComponent<ArrowPhysics>().speed = speed;
    }
}
