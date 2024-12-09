using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnemyShieldBehaviour : MonoBehaviour
{
    [SerializeField] private float angleLength;
    [SerializeField] private int positionAmount;
    [SerializeField] private float shieldRange;
    [SerializeField] private GameObject positionPrefabEmpty;
    [SerializeField] private float rotationSpeed;
    private List<Transform> _positionsForShield = new List<Transform>();
    private bool _shieldHit;

    [SerializeField] private Transform knockbackPosition;
    public Transform leftArm;

    private void Start()
    {
        leftArm = PlayerReferenceScript.Instance.transform;
        MakeAngle();

    }

    private void MakeAngle()
    {
        for (int i = 0; i < positionAmount; i++)
        {
            float angle = (angleLength / positionAmount) * i;
            float radian = angle * Mathf.Deg2Rad;

            Vector3 position = new Vector3(Mathf.Cos(radian) * shieldRange, 0, Mathf.Sin(radian) * shieldRange);

            GameObject newEmpty = Instantiate(positionPrefabEmpty, transform.parent.position + position, Quaternion.identity, transform.parent.parent);
            newEmpty.transform.LookAt(transform.parent.position);

            _positionsForShield.Add(newEmpty.transform);


        }
    }

    private void Update()
    {
        if (_shieldHit)
        {
            HandleShieldAway(knockbackPosition);
        }

        else
        {
            HoldingShieldAtPositions();
        }
        
        
    }

    private void HoldingShieldAtPositions()
    {
        TakeClosestPositionToWeapon();
    }

    private void TakeClosestPositionToWeapon()
    {
        Transform rightPosition = GetRightPosition();

        transform.position = Vector3.Lerp(transform.position, rightPosition.position, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rightPosition.rotation, rotationSpeed * Time.deltaTime);
    }

    private Transform GetRightPosition()
    {
        Transform position = null;
        float lowestDistance = Mathf.Infinity;
        
        for (int i = 0; i < _positionsForShield.Count; i++)
        {
            float distance = Vector3.Distance(_positionsForShield[i].position, leftArm.position);

            if (distance < lowestDistance)
            {
                lowestDistance = distance;
                position = _positionsForShield[i];
            }
        }

        return position;
    }

   

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Weapon") && !_shieldHit)
        {
            print("touched"); 
            transform.root.GetComponent<EnemyAI>().JumpFromDamage();
            _shieldHit = true;
            Invoke(nameof(ResetKnockback), 2f);

        }
    }

    private void HandleShieldAway(Transform position)
    {
        transform.position = Vector3.Lerp(transform.position, position.position, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, position.rotation, rotationSpeed * Time.deltaTime);
        

    }

    private void ResetKnockback()
    {
        _shieldHit = false;
    }

}
