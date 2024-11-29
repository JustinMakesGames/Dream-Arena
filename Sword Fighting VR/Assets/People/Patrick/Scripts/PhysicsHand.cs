using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicsHand : MonoBehaviour
{
    public Transform target;
    private Rigidbody _rb;
    private Rigidbody _heldItem;
    private ActionBasedController _actionBasedController;

    private void Start()
    {
        _actionBasedController = target.GetComponent<ActionBasedController>();
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = 100f;
    }
    
    private void FixedUpdate()
    {
        if (_heldItem != null && _heldItem != _actionBasedController.modelPrefab.GetComponent<Rigidbody>())
        {
            _heldItem = _actionBasedController.modelPrefab.GetComponent<Rigidbody>();
            _actionBasedController = target.GetComponent<ActionBasedController>();
        }
        _rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotationDif = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDif.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 rotationDifDegree = angle * axis;
        _rb.angularVelocity = rotationDifDegree * Mathf.Deg2Rad / Time.fixedDeltaTime;
        Vector3 vel = _rb.velocity;
        vel.y = Vector3.down.y * 9.81f - _heldItem.mass;
        _rb.velocity = vel;
    }
    
    
}
