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

    [Header("Items")]
    [SerializeField] private List<GameObject> possibleItems;
    [SerializeField] private Transform itemFolder;
    private List<Transform> itemPositions;
    private List<Transform> spawnedItems;

    public void StartGenerating()
    {
        SetEnemyPositionsInArray();
        ChooseEnemyGroup();
        SpawningEnemies();
        SpawnOccasionalItems();
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

    private void SpawnOccasionalItems()
    {
        spawnedItems.Clear();
        int randomChance = UnityEngine.Random.Range(0, 2);

        if (randomChance == 0) return;
        int randomAmount = UnityEngine.Random.Range(0, 3);
        int randomItem = UnityEngine.Random.Range(0, possibleItems.Count);
        int randomPosition = UnityEngine.Random.Range(0, itemPositions.Count);

        for (int i = 0; i < randomAmount; i++)
        {
            GameObject itemClone = Instantiate(possibleItems[randomItem], itemPositions[randomPosition].position, Quaternion.identity);
            spawnedItems.Add(itemClone.transform);

        }
    }





}
