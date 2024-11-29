using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct EnemyCombination
{
    public string groupName;
    public List<GameObject> possibleEnemies;

    public int minScoreCost;
    public int maxScoreCost;
    public enum EnemyOrder
    {
        Order,
        Random
    }

    public EnemyOrder orderState;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score;
    [HideInInspector] public int livingEnemyAmount;
    [SerializeField] private int maxRooms;

    public List<GameObject> enemies;

    private bool hasBattleBegun;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartBattle()
    {
        hasBattleBegun = true;
    }

    public void SetEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void InitializeBattle()
    {
        FindObjectOfType<HandleBattling>().enabled = true;
    }





}
