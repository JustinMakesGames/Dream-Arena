using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = 100f;
    }
    
    private void FixedUpdate()
    {
        _rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotationDif = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDif.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 rotationDifDegree = angle * axis;
        _rb.angularVelocity = rotationDifDegree * Mathf.Deg2Rad / Time.fixedDeltaTime;
    }
    
    
}
