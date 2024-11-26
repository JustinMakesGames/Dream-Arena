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
    protected bool ignoresArmor;
    protected int baseDamage;
    private void Start()
    {
        UpdateController();
    }

    public bool GetArmorBool()
    {
        return ignoresArmor;
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
