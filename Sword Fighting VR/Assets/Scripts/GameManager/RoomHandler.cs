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

    private int randomEnemyGroup;
    private List<Transform> enemyPositions = new List<Transform>();

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
            enemyPositions.Add(enemyPositionFolder.GetChild(i));
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
        randomEnemyGroup = Random.Range(0, possibleEnemyCombinations.Count);
        _chosenEnemyGroup = possibleEnemyCombinations[randomEnemyGroup];
    }

    private void SpawningEnemies()
    {
        if (_chosenEnemyGroup.orderState == EnemyCombination.EnemyOrder.Random)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                int randomEnemy = Random.Range(0, _chosenEnemyGroup.possibleEnemies.Count);
                int randomPosition = Random.Range(0, enemyPositions.Count);

                GameObject enemy = Instantiate(_chosenEnemyGroup.possibleEnemies[randomEnemy], enemyPositions[randomPosition].position, Quaternion.identity);
                GameManager.Instance.SetEnemy(enemy);
            }
        }
    }

    private void HandleBeginningBattle()
    {
        GameManager.Instance.StartBattle();
    }



}
