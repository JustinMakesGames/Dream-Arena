using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum Rarity {
    COMMON,
    UNCOMMON,
    RARE,
    LEGENDARY
}

[Serializable]
public class LootManager : MonoBehaviour
{
    public Loot[] possibleLoot;
    public static LootManager Instance;
    public Dictionary<Rarity, float> rarityMap;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;

        rarityMap = new Dictionary<Rarity, float>
        {
            { Rarity.COMMON, 0.50f }, //50%
            { Rarity.UNCOMMON, 0.25f }, //25%
            { Rarity.RARE, 0.20f }, //20%
            { Rarity.LEGENDARY, 0.05f }, //5%
        };
    }
    
    public Loot GetLoot()
    {
        Rarity lootRarity = GetRandomRarity(rarityMap);
        print(lootRarity.ToString());
        Loot[] possibleRarityLoot = possibleLoot.Where(x => x.rarity == lootRarity).ToArray();
        return possibleRarityLoot[Random.Range(0, possibleRarityLoot.Length)];

    }
    
    public Rarity GetRandomRarity(Dictionary<Rarity, float> weights)
    {
        float totalWeight = 0f;
        foreach (var weight in weights.Values)
        {
            totalWeight += weight;
        }

        float random = Random.value * totalWeight;
        float cumulativeWeight = 0f;
        foreach (var key in weights)
        {
            cumulativeWeight += key.Value;
            if (random <= cumulativeWeight)
            {
                return key.Key;
            }
        }

        throw new Exception("Failed to get random loot drops");
    }
}
