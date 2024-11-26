using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public abstract class Weapon : MonoBehaviour
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
    private void Start()
    {
        UpdateController();
    }
    
    private void OnTransformParentChanged()
    {
        UpdateController();
    }
    
    private void UpdateController()
    {
        controller = transform.parent.GetComponent<ActionBasedController>();
        inputActionPosition = controller.positionAction;
        inputActionRotation = controller.rotationAction;
    }
    public abstract int GetDamage(Collider collision);
}
