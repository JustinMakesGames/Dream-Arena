using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XRController = UnityEngine.InputSystem.XR.XRController;

public class Weapon : MonoBehaviour
{
    public bool ignoresArmor;
    //Again, we don't want other scripts to be able to edit the damage variable directly, so we're only going to make it readable.
    [SerializeField] private int baseDamage;
    [SerializeField] private int maxDamage, minDamage;
    private Rigidbody _rb;
    private float _timeSinceLastDamage;
    private float _timePunishment;
    private InputActionProperty _inputActionPosition;
    private InputActionProperty _inputActionRotation;
    private ActionBasedController _controller;

    private void Start()
    {
        UpdateController();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTransformParentChanged()
    {
        UpdateController();
    }

    private void UpdateController()
    {
        _controller = transform.parent.GetComponent<ActionBasedController>();
        _inputActionPosition = _controller.positionAction;
        _inputActionRotation = _controller.rotationAction;
    }

    private void Update()
    {
        AddTime();
    }

    //We make sure the player has to swing harder in order to do more damage
    //We also add a punishment if you try to attack too quickly.
    public int GetDamage() 
    {
        Vector3 normalizedInput = _inputActionPosition.action.ReadValue<Vector3>().normalized 
                                  + _inputActionRotation.action.ReadValue<Quaternion>().eulerAngles.normalized;
        int damage = Mathf.CeilToInt(baseDamage * normalizedInput.magnitude);
        _timePunishment = _timeSinceLastDamage switch
        {
            <= 0.5f => 0f,
            <= 1 => 0.5f,
            _ => 1f
        };
        damage = Mathf.CeilToInt(damage * _timePunishment);
        _timeSinceLastDamage = 0;
        return damage;
    }

    private void AddTime()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }
}
