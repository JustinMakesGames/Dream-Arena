using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{

    public Transform enemyPositionFolder;
    public int enemyAmount;

    private List<Transform> enemyPositions;

    private void Start()
    {
        SetEnemyPositionsInArray();
        SpawningEnemies();
    }

    private void SetEnemyPositionsInArray()
    {
        for (int i = 0; i < enemyPositionFolder.childCount; i++)
        {
            enemyPositions.Add(enemyPositionFolder.GetChild(i).transform);
        }
    }

    private void SpawningEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            
        }
    }

}
