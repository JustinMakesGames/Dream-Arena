using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    
    [SerializeField] private InputActionProperty moveJoystick;
    [SerializeField] private InputActionProperty rotateJoystick;
    [SerializeField] private InputActionProperty up;
    [SerializeField] private InputActionProperty down;
    [SerializeField] private Transform cam;

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    private Quaternion _originalRotation;
    private Vector3 _direction;
    private Vector2 _rotation;

    private float _xRotation;
    private Rigidbody _rb;
    private float _up;
    private float _down;

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
        _up = up.action.ReadValue<float>();
        _down = down.action.ReadValue<float>();
    }

    private void MovingAround()
    {
        Vector3 directionToLook = cam.forward * _direction.y + cam.right * _direction.x;
        directionToLook.y = 0;

        if (_direction.x != 0 || _direction.y != 0)
        {
            _rb.MovePosition(transform.position + directionToLook * speed * Time.deltaTime);
        }
        if (_up != 0)
        {
            Vector3 movePos = new Vector3(transform.position.x,
                transform.position.y + _up * speed * Time.deltaTime,
                transform.position.z);
            _rb.MovePosition(movePos);
        }
        if (_down != 0)
        {
            Vector3 movePos = new Vector3(transform.position.x,
                transform.position.y - _down * speed * Time.deltaTime,
                transform.position.z);
            _rb.MovePosition(movePos);
        }
    }

    private void RotateAround()
    {
        _xRotation += _rotation.x * rotateSpeed;
        

        cam.rotation = Quaternion.Euler(cam.rotation.x, _xRotation, cam.rotation.z);       
    }

}
