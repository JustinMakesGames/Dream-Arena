using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]

public class Weapon : MonoBehaviour
{
    protected string[] cuttableLimbs =
    {
        "Head",
        "Left_Arm",
        "Right_Arm",
        "Left_Leg",
        "Right_Leg",
    };
    
    [Header("Controllers")]
    public InputActionProperty inputActionPosition;
    public InputActionProperty inputActionRotation;
    public ActionBasedController controller;
    [SerializeField] private Transform hand;
    [Header("Weapon Information")]
    public WeaponSO weaponSo;
    //Make sure these variables can only be accessed from classes which derive from this one,
    //while still making sure other scripts can get the values.
    protected bool ignoresArmor;
    public bool IgnoresArmor() => ignoresArmor;
    protected int baseDamage;
    public int GetDamage() => baseDamage;
    public bool isEquipped;


    private UnityEngine.XR.InputDevice _controller;

    private Vector3 previousPosition;
    private Vector3 velocity;

    private float _time;
    private bool _isHit;
    protected Rigidbody rb;
    
    protected virtual void Start()
    {
        _controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    protected virtual void Update()
    {
        CalculatingHandVelocity();
        HittingTimePunishment();
        
    }

    private void CalculatingHandVelocity()
    {
        Vector3 currentPosition = inputActionPosition.action.ReadValue<Vector3>();
        velocity = (currentPosition - previousPosition) / Time.deltaTime;
        
        previousPosition = currentPosition;
    }

    private void HittingTimePunishment()
    {
        
        if (_isHit)
        {
            _time += Time.deltaTime;

            if (_time > weaponSo.timePunishment)
            {
                _isHit = false;
                _time = 0;
            }
        }
    }

    public void UpdateController(Transform selectedHand)
    {
        controller = selectedHand.GetComponent<PhysicsHand>().target.GetComponent<ActionBasedController>();
        inputActionPosition = controller.positionAction;
        inputActionRotation = controller.rotationAction;
    }

    public virtual int GetDamage(Collider collision)
    {
        if (_isHit) return 0;
        _isHit = true;
        if (velocity.magnitude > 1)
        {
            int damage = CalculateDamage(velocity.magnitude, weaponSo.damage);
            return damage;
        }
        return weaponSo.damage;
    }

    private int CalculateDamage(float velocityMagnitude, int baseDamage)
    {
        int multiplier = 0;
        switch (velocityMagnitude)
        {
            case <= 3:
                multiplier = 1;
                break;
            case <= 5:
                multiplier = 2;
                break;
            case > 5:
                multiplier = 3;
                break;
        }
        int damage = baseDamage * multiplier;

        return damage;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (cuttableLimbs.Contains(collision.gameObject.name.ToLower()))
        {
            if (rb.velocity.magnitude > 2)
            {
                GameObject obj = collision.gameObject;
                collision.gameObject.GetComponent<EnemyHealth>().LoseLimb(obj);
            }
        }
    }
    
}
