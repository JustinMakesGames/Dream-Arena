using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    
    public InputActionProperty moveJoystick;

    [SerializeField] private float speed;

    private Vector2 _direction;

    private void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        _direction = moveJoystick.action.ReadValue<Vector2>();

        Vector3 directionToGo = new Vector3(_direction.x, 0, _direction.y);

        transform.Translate(directionToGo * speed * Time.deltaTime);

    }

}
