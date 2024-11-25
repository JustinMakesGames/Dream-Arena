using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : Weapon
{
    [SerializeField] private bool ignoresArmor;
    [SerializeField] private int baseDamage;
    private float _timeSinceLastDamage;
    private Vector3 _lastInputRot;
    
    #region Damage Handler
    public override int GetDamage(Collider collision)
    {
        Vector3 rot = inputActionRotation.action.ReadValue<Quaternion>().eulerAngles.normalized;
        Vector3 normalizedInput = inputActionPosition.action.ReadValue<Vector3>().normalized + rot;
        
        float timePunishment = TimePunishment();
        float distancePunishment = DistancePunishment(collision);
        
        if (_timeSinceLastDamage <= 0.1f) return 0;
        if (rot == _lastInputRot) return 0;
        int damage = Mathf.RoundToInt(baseDamage * normalizedInput.magnitude 
                                                  * distancePunishment * timePunishment);
        _lastInputRot = rot;
        _timeSinceLastDamage = 0;
        return 100;
    }
    #endregion
    
    #region Punishments
    private float TimePunishment()
    {
        float timePunishment = _timeSinceLastDamage switch
        {
            <= 0.2f => 0f,
            <= 0.5f => 0.5f,
            _ => 1f
        };
        return timePunishment;
    }

    private float DistancePunishment(Collider collision)
    {
        float distancePunishment = Vector3.Distance(transform.position, collision.transform.position) switch
        {
            0 => 2,
            <= 1 => 1, 
            <= 6 => 0.5f,
            _ => 0.1f
        };
        return distancePunishment;
    }
    #endregion
}
