using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    
    public InputActionProperty moveJoystick;
    public InputActionProperty rotateJoystick;

    public Transform cam;

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    private Quaternion _originalRotation;
    private Vector2 _direction;
    private Vector2 _rotation;

    private float _xRotation;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        CheckInputs();
        MovingAround();
        RotateAround();
    }

    private void CheckInputs()
    {
        _direction = moveJoystick.action.ReadValue<Vector2>();
        _rotation = rotateJoystick.action.ReadValue<Vector2>();
    }

    private void MovingAround()
    {


        Vector3 directionToLook = cam.forward * _direction.y + cam.right * _direction.x;
        directionToLook.y = 0;

        _rb.velocity = directionToLook.normalized * speed * Time.deltaTime;
    }

    private void RotateAround()
    {
        _xRotation += _rotation.x * rotateSpeed;
        

        cam.rotation = Quaternion.Euler(cam.rotation.x, _xRotation, cam.rotation.z);       
    }

}
