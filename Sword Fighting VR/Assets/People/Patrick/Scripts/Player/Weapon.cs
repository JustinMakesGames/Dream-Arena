using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Weapon : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [Tooltip("If this bool is checked, the weapon ignores any armor damage reduction.")]
    [SerializeField] private bool ignoresArmor;
    //Again, we don't want other scripts to be able to edit the damage variable directly, so we're only going to make it readable.
    [SerializeField] private int baseDamage;
    private Rigidbody _rb;
    private float _timeSinceLastDamage;
    private float _timePunishment;
    private InputActionProperty _inputActionPosition;
    private InputActionProperty _inputActionRotation;
    private ActionBasedController _controller;
    private float _distancePunishment;
    private Vector3 _lastInputRot;
    #endregion
    
    #region Setup
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
    #endregion

    #region Time Handling
    private void Update()
    {
        AddTime();
    }
    public void AddTime()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }
    #endregion
    
    #region Damage Handling
    //This makes sure the player has to swing harder in order to do more damage
    //This also adds a punishment if you try to attack too quickly.
    public int GetDamage(Collider collision)
    {
        _distancePunishment = Vector3.Distance(transform.position, collision.transform.position) switch
        {
            0 => 2,
            <= 1 => 1, 
            <= 6 => 0.5f,
            _ => 0.1f
        };
        _timePunishment = _timeSinceLastDamage switch
        {
            <= 0.2f => 0f,
            <= 0.5f => 0.5f,
            _ => 1f
        };
        Vector3 rot = _inputActionRotation.action.ReadValue<Quaternion>().eulerAngles.normalized;
        Vector3 normalizedInput = _inputActionPosition.action.ReadValue<Vector3>().normalized + rot;
        if (_timeSinceLastDamage <= 0.1f) return 0;
        if (rot == _lastInputRot) return 0;
        _lastInputRot = rot;
        int damage = Mathf.CeilToInt(baseDamage / normalizedInput.magnitude);
        damage = Mathf.CeilToInt(damage * _distancePunishment * _timePunishment);
        _timeSinceLastDamage = 0;
        return damage;
    }
    #endregion
}
