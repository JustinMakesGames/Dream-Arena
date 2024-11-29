using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour {
    private Animator _animator;
    private bool _ikActive = false;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform lookObj = null;
    [SerializeField] private GameObject player;
    private Vector3 _currentPos;
    private bool _hasBlocked;
    private bool _isFrozen;
    private Vector3 _frozenRightHandPosition;
    private Quaternion _frozenRightHandRotation;
    [SerializeField] private float timeBetweenBlocks;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _ikActive = Vector3.Distance(transform.position, FindActiveWeapon().transform.position) <= 5;
        if (_ikActive)
        {
            rightHandObj = FindActiveWeapon().transform.GetChild(0);
            lookObj = rightHandObj;
        }
    }

    private Weapon FindActiveWeapon() => FindObjectsByType<Weapon>(FindObjectsSortMode.None).First(x => x.isEquipped);

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_animator) return;
        if(_ikActive)
        {
            if(lookObj) {
                _animator.SetLookAtWeight(1);
                _animator.SetLookAtPosition(lookObj.position);
            }

            if (!rightHandObj) return;
            if (!_isFrozen)
            {
                _currentPos = _animator.GetIKPosition(AvatarIKGoal.RightHand);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                FreezeIK();
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, _frozenRightHandPosition - new Vector3(0, 0, 180));
                _animator.SetIKRotation(AvatarIKGoal.RightHand, _frozenRightHandRotation);
            }
        }
        else
        {
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetLookAtWeight(0);
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