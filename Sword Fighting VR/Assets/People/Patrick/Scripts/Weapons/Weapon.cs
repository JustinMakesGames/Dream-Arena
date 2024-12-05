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
    public bool isEquipped;
    
    
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

    private void OnCollisionEnter(Collision other)
    {
        print("Collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(500);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(500);
        }
    }

    public virtual int GetDamage(Collider collision)
    {
        return weaponSo.damage;
    }
    
    
}
