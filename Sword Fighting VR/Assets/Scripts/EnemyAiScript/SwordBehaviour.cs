using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SwordBehaviour : MonoBehaviour
{
    [Header("Holding Sword")]
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    private float _originalYPosition;
    private float _sineWave;

    [Header("Pointing Sword")]
    [SerializeField] private float pointAmplitude;
    [SerializeField] private float pointFrequency;
    [SerializeField] private float angle;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float radius;

    [Header("Attacking")]
    [SerializeField] private Animator slashingAnimator;
    [SerializeField] private bool isReadyForAttack;

    [SerializeField] private EnemyAI enemyAIScript;

    private EnemyStats _enemyStats;

    private void Awake()
    {
        _originalYPosition = transform.position.y;
        enemyAIScript = transform.root.GetComponent<EnemyAI>();
        _enemyStats = enemyAIScript.enemyStats;
    }

    public void StartAttacking()
    {
        StartCoroutine(AttackIntervals());

    }
    private void Update()
    {
        HoldingSwordAnimation();
        
    }

    private void HoldingSwordAnimation()
    {
        _sineWave = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, _originalYPosition + _sineWave, transform.position.z);
    }

    private void CalculateAngle()
    {
        float radians = angle * Mathf.Deg2Rad; // Convert angle to radians
        Vector3 direction = Quaternion.Euler(0, angle, 0) * centerPoint.forward;

        // Calculate the new position
        Vector3 offset = direction.normalized * radius;
        transform.position = new Vector3(centerPoint.position.x, transform.position.y, centerPoint.position.z) + offset;
    }

    private void ChangingAngle()
    {
        angle = Mathf.Cos(Time.time * pointFrequency) * pointAmplitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(_enemyStats.damage);
            print("Enemy did: " + _enemyStats.damage + " damage. The player has " + other.GetComponent<PlayerHealth>().health + " left.");
        }
    }

    private IEnumerator AttackIntervals()
    {
        yield return new WaitForSeconds(0.5f);
        while (enemyAIScript.isAttacking)
        {
            slashingAnimator.SetTrigger("RightSlashTrigger");
            yield return new WaitForSeconds(0.5f);

        }


    }


}
