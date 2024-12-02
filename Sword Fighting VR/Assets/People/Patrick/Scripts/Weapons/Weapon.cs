using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]

public class Weapon : MonoBehaviour
{
    [HideInInspector] public InputActionProperty inputActionPosition;
    [HideInInspector] public InputActionProperty inputActionRotation;
    [HideInInspector] public ActionBasedController controller;
    public WeaponSO weaponSo;
    //Make sure these variables can only be accessed from classes which derive from this one,
    //while still making sure other scripts can get the values.
    protected bool ignoresArmor;
    public bool IgnoresArmor() => ignoresArmor;
    protected int baseDamage;
    public int GetDamage() => baseDamage;
    public bool isEquipped;
    
    protected virtual void Start()
    {
        UpdateController();
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
        return weaponSo.damage;
    }

    
}
