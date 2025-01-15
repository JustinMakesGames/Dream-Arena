using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject fireball;
    public bool isAlive;
    [SerializeField] private int health;
    public int GetHealth() => health;
    private NavMeshAgent _navMeshAgent;
    private float _speed;

    private void Start()
    {
        StartCoroutine(AttackPattern());
    }

    private void Update()
    {
        if (!isAlive)
        {
            GameManager.Instance.HandleWinning();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            isAlive = false;
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        throw new NotImplementedException();
    }
    private IEnumerator AttackPattern()
    {
        while (isAlive)
        {
            _speed = health switch
            {
                0 => 0,
                < 20 => Random.Range(0.1f, 0.9f),
                < 40 => Random.Range(1, 2),
                < 60 => Random.Range(2, 3),
                < 80 => Random.Range(3, 4),
                _ => Random.Range(5, 6)
            };
            yield return new WaitForSeconds(_speed);
            Instantiate(fireball, transform.position, transform.rotation);
            
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
        }
    }
}
