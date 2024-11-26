using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public struct EnemyCombination
{
    public string groupName;
    public GameObject possibleEnemies;

    public int scoreCost;
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
    [SerializeField] private int roomNumber;
    [SerializeField] private int maxRooms;

    
    [HideInInspector] public int livingEnemyAmount;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    

    


}
