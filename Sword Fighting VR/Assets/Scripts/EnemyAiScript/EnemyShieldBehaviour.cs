using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldBehaviour : MonoBehaviour
{
    [SerializeField] private float angleLength;
    [SerializeField] private int positionAmount;
    [SerializeField] private float shieldRange;
    [SerializeField] private GameObject positionPrefabEmpty;
    private List<Transform> _positionsForShield = new List<Transform>();

    private void Start()
    {
        MakeAngle();
    }

    private void MakeAngle()
    {
        for (int i = 0; i < positionAmount; i++)
        {
            float angle = (angleLength / positionAmount) * i;
            float radian = angle * Mathf.Deg2Rad;

            Vector3 position = new Vector3(Mathf.Cos(radian) * shieldRange, 0, Mathf.Sin(radian) * shieldRange);

            Vector3 direction = (transform.parent.position - position).normalized;
            direction.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject newEmpty = Instantiate(positionPrefabEmpty, transform.parent.position + position, Quaternion.identity, transform.parent);

            _positionsForShield.Add(newEmpty.transform);


        }
    }

}
