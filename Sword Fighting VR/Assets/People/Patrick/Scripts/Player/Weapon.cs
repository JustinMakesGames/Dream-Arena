using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [SerializeField] private bool ignoresArmor;
    //Again, we don't want other scripts to be able to edit the damage variable directly, so we're only going to make it readable.
    [SerializeField] private int baseDamage;
    [SerializeField] private int maxDamage, minDamage;
    private Rigidbody _rb;
    private float _timeSinceLastDamage;
    private float _timePunishment;
    private InputActionProperty _inputActionPosition;
    private InputActionProperty _inputActionRotation;
    private ActionBasedController _controller;
    private float _distancePunishment;
    private Vector3 _lastInputRot;
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
    public int GetDamage(Collider collision)
    {
        Vector3 rot = _inputActionRotation.action.ReadValue<Quaternion>().eulerAngles.normalized;
        if (_timeSinceLastDamage <= 0.1f) return 0;
        Vector3 normalizedInput = _inputActionPosition.action.ReadValue<Vector3>().normalized + rot;
        if (rot == _lastInputRot) return 0;
        _lastInputRot = rot;
        int damage = Mathf.CeilToInt(baseDamage / normalizedInput.magnitude);
        _distancePunishment = Vector3.Distance(transform.position, collision.transform.position) switch
        {
            0 => 2,
            <= 1 => 1, 
            <= 3 => 0.5f,
            _ => 0.1f
        };
        _timePunishment = _timeSinceLastDamage switch
        {
            <= 0.3f => 0f,
            <= 0.6f => 0.5f,
            _ => 1f
        };
        damage = Mathf.CeilToInt(damage * _distancePunishment);
        damage = Mathf.CeilToInt(damage * _timePunishment);
        _timeSinceLastDamage = 0;
        return damage;
    }

    private void AddTime()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }
}
