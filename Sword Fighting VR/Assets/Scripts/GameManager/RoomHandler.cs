using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private List<EnemyCombination> allEnemyCombinations;
    [SerializeField] private List<EnemyCombination> possibleEnemyCombinations;

    private EnemyCombination _chosenEnemyGroup;
    public Transform enemyPositionFolder;
    public int enemyAmount;

    private int _randomEnemyGroup;
    private List<Transform> _enemyPositions = new List<Transform>();

    private void Start()
    {
        SetEnemyPositionsInArray();
        ChooseEnemyGroup();
        SpawningEnemies();
        HandleBeginningBattle();
    }

    public void StartGenerating()
    {
        
    }

    private void SetEnemyPositionsInArray()
    {
        for (int i = 0; i < enemyPositionFolder.childCount; i++)
        {
            _enemyPositions.Add(enemyPositionFolder.GetChild(i));
        }
    }

    private void ChooseEnemyGroup()
    {
        foreach (EnemyCombination comb in allEnemyCombinations)
        {
            if (GameManager.Instance.score >= comb.minScoreCost && GameManager.Instance.score <= comb.maxScoreCost)
            {
                possibleEnemyCombinations.Add(comb);
            }
        }
        _randomEnemyGroup = Random.Range(0, possibleEnemyCombinations.Count);
        _chosenEnemyGroup = possibleEnemyCombinations[_randomEnemyGroup];
    }

    private void SpawningEnemies()
    {
        if (_chosenEnemyGroup.orderState == EnemyCombination.EnemyOrder.RANDOM)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                int randomEnemy = Random.Range(0, _chosenEnemyGroup.possibleEnemies.Count);
                int randomPosition = Random.Range(0, _enemyPositions.Count);

                GameObject enemy = Instantiate(_chosenEnemyGroup.possibleEnemies[randomEnemy], _enemyPositions[randomPosition].position, Quaternion.identity);
                GameManager.Instance.SetEnemy(enemy);
            }
        }
    }

    private void HandleBeginningBattle()
    {
        GameManager.Instance.StartBattle();
    }



}
