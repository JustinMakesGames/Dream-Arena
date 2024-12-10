using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Loot", menuName = "Loot")]
public class Loot : ScriptableObject
{
    public GameObject prefab;
    public Color color;
    public Rarity rarity;
}
