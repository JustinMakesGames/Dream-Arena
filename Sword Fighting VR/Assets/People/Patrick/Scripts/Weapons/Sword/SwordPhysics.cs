using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections;

public class SwordPhysics : MonoBehaviour
{
    private ActionBasedController _controller;
    private InputActionProperty _inputActionRotation;
    private InputActionProperty _inputActionPosition;
    private PhysicsHand _rbHand;
    private Rigidbody _rb;
    private Transform _origin;

    private void OnTransformParentChanged()
    {
        _rbHand = GetComponentInParent<PhysicsHand>();
        _rb = _rbHand.GetComponent<Rigidbody>();
        _controller = _rbHand.target.GetComponent<ActionBasedController>();
        _inputActionPosition = _controller.positionAction;
        _inputActionRotation = _controller.rotationAction;
    }

    public void FixedUpdate()
    {
        if (!_rb) return;
        Vector3 normalizedInput = (_inputActionPosition.action.ReadValue<Vector3>() 
                                  + _inputActionRotation.action.ReadValue<Vector3>()).normalized;
        if (_rb.velocity.magnitude > 0.1f && normalizedInput.magnitude > 0.5f)
        {
            if (_origin.position != Vector3.zero && _origin.rotation != Quaternion.identity)
            {
                _origin.position = _rb.position;
                _origin.rotation = _rb.rotation;
                StartCoroutine(SlerpToPosition(_origin.position, _rb.position, Time.deltaTime / _rb.mass));
                StartCoroutine(SlerpToRotation(_origin.rotation, _rb.rotation, Time.deltaTime / _rb.mass));
            }
        }
    }

    IEnumerator SlerpToPosition(Vector3 from, Vector3 to, float t)
    {
        while (Vector3.Distance(from, to) > 0.1f)
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
