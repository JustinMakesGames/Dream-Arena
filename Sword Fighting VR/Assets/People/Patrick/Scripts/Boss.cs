using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject fireball;
    public bool isAlive;
    [SerializeField] private int health;
    public int GetHealth() => health;
    [SerializeField] private NavMeshSurface surface;
    private float _speed;
    [SerializeField] private float teleportDistance;

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

        if (health > 0) return;
        isAlive = false;
        StartCoroutine(DeathAnimation());
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
            transform.parent.position = GetRandomPosition();
            transform.parent.LookAt(GameManager.Instance.player.position);
        }
    }

    private Vector3 GetRandomPosition()
    {
        teleportDistance = health switch
        {
            0 => 0,
            <= 20 => Random.Range(40, 45),
            <= 40 => Random.Range(35, 40),
            <= 60 => Random.Range(30, 35),
            <= 80 => Random.Range(25, 30),
            _ => 25
        };
        Vector3 randomDirection = Random.insideUnitCircle * teleportDistance;
        randomDirection.z = randomDirection.y;
        randomDirection.y = 0;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, teleportDistance, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }
}
