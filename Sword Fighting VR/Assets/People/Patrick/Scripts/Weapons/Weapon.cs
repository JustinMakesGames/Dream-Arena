using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]

public class Weapon : MonoBehaviour
{
    public InputActionProperty inputActionPosition;
    public InputActionProperty inputActionRotation;
    public ActionBasedController controller;
    public WeaponSO weaponSo;
    //Make sure these variables can only be accessed from classes which derive from this one,
    //while still making sure other scripts can get the values.
    protected bool ignoresArmor;
    public bool IgnoresArmor() => ignoresArmor;
    protected int baseDamage;
    public int GetDamage() => baseDamage;
    public bool isEquipped;

    [SerializeField] private Transform hand;
    private UnityEngine.XR.InputDevice _controller;

    private Vector3 previousPosition;
    private Vector3 velocity;

    private float time;
    private bool isHit;
    
    protected virtual void Start()
    {
        _controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        UpdateController();
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
        if (isHit)
        {
            time += Time.deltaTime;

            if (time > weaponSo.timePunishment)
            {
                isHit = false;
                time = 0;
            }
        }
    }
    private void OnTransformParentChanged()
    {
        UpdateController();
    }

    private void UpdateController()
    {
        controller = transform.GetComponent<PhysicsHand>().target.GetComponent<ActionBasedController>();
        inputActionPosition = controller.positionAction;
        inputActionRotation = controller.rotationAction;
    }

    public virtual int GetDamage(Collider collision)
    {
        if (isHit) return 0;
        isHit = true;
        print("Tries damage");
        if (velocity.magnitude > 1)
        {
            return weaponSo.damage;
        }
        print("no damage");
        return 0;
    }

    
}
