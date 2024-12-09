using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringInteractable : XRGrabInteractable
{
    private Transform _tensedStringTransform;
    [SerializeField] private bool hasArrow;
    private void Start()
    {
        _tensedStringTransform = transform;
    }

    protected override void Drop()
    {
        transform.position = new Vector3(0, 0.0055f, 0);
        transform.rotation = Quaternion.identity;
        if (hasArrow)
        {
            GetComponent<Bow>().Shoot();
        }
    }
}
