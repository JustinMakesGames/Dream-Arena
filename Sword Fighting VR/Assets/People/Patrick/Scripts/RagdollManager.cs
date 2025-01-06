using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private static List<GameObject> _ragdolls = new();
    public static RagdollManager Instance;
    private int _maxAmountOfRagdolls;

    private void Awake()
    { 
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CleanRagdolls());
    }

    public static void SpawnRagdoll(EnemyHealth enemy)
    {
        GameObject ragdoll = Instantiate(enemy.ragdoll, enemy.transform);
        if (enemy.hasLostLimb)
        {
            foreach (GameObject lostLimb in enemy.lostLimbs)
            {
                Destroy(ragdoll.GetNamedChild(lostLimb.name));
            }
        }
        _ragdolls.Add(ragdoll);
    }

    IEnumerator CleanRagdolls()
    {
        while (true)
        {
            if (_ragdolls.Count >= _maxAmountOfRagdolls)
            {
                _ragdolls.RemoveAt(0);
            }
        }
    }
}
