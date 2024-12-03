using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour {
    private Animator _animator;
    private bool _ikActive = false;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform lookObj = null;
    [SerializeField] private GameObject player;
    private Vector3 _currentPos;
    private EnemyStats _stats;
    private bool _hasBlocked;
    private bool _isFrozen;
    private Vector3 _frozenRightHandPosition;
    [SerializeField] private float timeBetweenBlocks;
    [SerializeField] private float distance;
    private bool _doesPlayerHaveAWeapon;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (FindActiveWeapon() != null)
        {
            _ikActive = Vector3.Distance(transform.position, FindActiveWeapon().transform.position) <= distance;
        }
        else _ikActive = false;
        if (_ikActive)
        {
            rightHandObj = FindActiveWeapon().transform;
            lookObj = rightHandObj;
        }
    }
    
    private Weapon FindActiveWeapon()
    {
        Weapon[] foundWeapons = FindObjectsOfType<Weapon>();
        return foundWeapons.Length == 0 ? null : foundWeapons.FirstOrDefault(x => x.isEquipped);
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator) 
        {
            if(_ikActive)
            {
                if(lookObj != null) {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(lookObj.position);
                }
                if(rightHandObj != null)
                {
                    if (!_isFrozen)
                    {
                        _currentPos = _animator.GetIKPosition(AvatarIKGoal.RightHand);
                        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                        FreezeIK();
                    }
                    else
                    {
                        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);  
                        _animator.SetIKPosition(AvatarIKGoal.RightHand, _frozenRightHandPosition);
                    }
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetLookAtWeight(0);
            }
        }
    }
    
    private void FreezeIK()
    {
        _frozenRightHandPosition = _animator.GetIKPosition(AvatarIKGoal.RightHand);
        _isFrozen = true;
        StartCoroutine(BlockCooldown());
    }
    
    private void UnfreezeIK()
    {
        _isFrozen = false;
    }
    
    public IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(timeBetweenBlocks);
        UnfreezeIK();
        _hasBlocked = false;
    }
    
}