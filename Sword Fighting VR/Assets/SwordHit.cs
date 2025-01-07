using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private EnemyStats enemyStats;

    private void Start()
    {
        enemyStats = transform.root.GetComponent<EnemyAI>().enemyStats;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(enemyStats.damage);
        }
    }
}
