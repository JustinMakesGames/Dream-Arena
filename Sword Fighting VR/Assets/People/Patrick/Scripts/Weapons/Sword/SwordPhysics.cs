using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SwordPhysics : MonoBehaviour
{
    private ActionBasedController _controller;
    private InputActionProperty _inputActionRotation;
    private InputActionProperty _inputActionPosition;
    private PhysicsHand _rbHand;
    private Rigidbody _rb;
    private Transform _origin;
    private List<Coroutine> _currentlyActiveCoroutines = new();
    private Weapon _weapon;

    private void Start() 
    {
        StartCoroutine(PhysicsHandling());
    }
    private void OnTransformParentChanged()
    {
        _weapon = GetComponent<Weapon>();
        if (!_weapon.isEquipped) return;
        _rbHand = GetComponentInParent<PhysicsHand>();
        _rb = _rbHand.GetComponent<Rigidbody>();
        _controller = _rbHand.target.GetComponent<ActionBasedController>();
        _inputActionPosition = _controller.positionAction;
        _inputActionRotation = _controller.rotationAction;
        
    }

    IEnumerator PhysicsHandling()
    {
        while (true)
        {
            yield return new WaitUntil(() => _weapon.isEquipped);
            _origin = _rb.transform;
            yield return new WaitUntil(() => _origin.position != _rb.position);
            
            StartCoroutine(SlerpToPosition(_origin.position, _rb.position, 
                (Time.deltaTime / _rb.mass) * _rb.velocity.magnitude)); 
            StartCoroutine(SlerpToRotation(_origin.rotation, _rb.rotation, 
                (Time.deltaTime / _rb.mass) * _rb.velocity.magnitude));
            yield return new WaitForFixedUpdate();
            
        }
    }
    IEnumerator SlerpToPosition(Vector3 from, Vector3 to, float t)
    {
        while (transform.position != to)
        {
            transform.position = Vector3.Slerp(from, to, t);
            yield return null;
        }
    }
    IEnumerator SlerpToRotation(Quaternion from, Quaternion to, float t)
    {
        while (transform.rotation != to)
        {
            transform.rotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
    }
    
}
