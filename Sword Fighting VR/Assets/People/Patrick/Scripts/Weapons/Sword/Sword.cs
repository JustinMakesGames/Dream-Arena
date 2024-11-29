using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : Weapon
{
    #region Variables
    private float _timeSinceLastDamage;
    private Vector3 _lastInputRot;
    protected override void Start()
    {
        base.Start();
        baseDamage = weaponSo.damage;
        ignoresArmor = weaponSo.ignoresArmor;
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }

    #endregion

    #region Damage Handler

    public override int GetDamage(Collider collision)
    {
        Vector3 rot = inputActionRotation.action.ReadValue<Quaternion>().eulerAngles.normalized;
        Vector3 normalizedInput = inputActionPosition.action.ReadValue<Vector3>().normalized + rot;

        print("Rotation: " + rot.magnitude);
        print("NormalizedInput: " + normalizedInput.magnitude);
        float timePunishment = TimePunishment();

        print("TimePunishment " + timePunishment);

        if (_timeSinceLastDamage <= 0.1f) return 0;
        if (rot == _lastInputRot) return 0;
        int damage = Mathf.RoundToInt(baseDamage * normalizedInput.magnitude);
        damage = Mathf.RoundToInt(damage * timePunishment);
        _lastInputRot = rot;
        _timeSinceLastDamage = 0;
        print(damage);
        return damage;
    }

    #endregion

    #region Punishments

    private float TimePunishment()
    {
        switch (_timeSinceLastDamage)
        {
            case <= 0.2f:
                return 0f;
            case <= 0.5f:
                return 0.5f;
            default:
                return 1f;
        }
    }

    #endregion
}