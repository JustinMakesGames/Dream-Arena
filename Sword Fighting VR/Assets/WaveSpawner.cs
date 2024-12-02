using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleEnemies;
    [SerializeField] private int amountOfWaves;
    private bool _isSpawning;
    private int _amountOfEnemiesPerWave;
    private List<GameObject> _currentEnemies = new();
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < amountOfWaves; i++)
        {
            while (_currentEnemies.Count < _amountOfEnemiesPerWave)
            {
                GameObject enemy = Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Length)], transform.position, Quaternion.identity);
                _currentEnemies.Add(enemy);
                yield return new WaitUntil(() => Vector3.Distance(enemy.transform.position, transform.position) < 1.5f);
            } 
        }
    }
}
