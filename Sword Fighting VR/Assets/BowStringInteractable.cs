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
        transform.position = _tensedStringTransform.position;
        transform.rotation = _tensedStringTransform.rotation;
        if (hasArrow)
        {
            transform.root.GetComponent<Bow>().Shoot();
        }
    }
}
