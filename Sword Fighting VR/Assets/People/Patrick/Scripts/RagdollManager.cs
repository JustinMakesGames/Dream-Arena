using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

public class RagdollManager : MonoBehaviour
{
    private static List<GameObject> _ragdolls = new();
    public static RagdollManager Instance;
    [SerializeField] private int maxAmountOfRagdolls;

    private void Awake()
    { 
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CleanRagdolls());
    }

    public static void SpawnRagdoll(GameObject oldEnemy)
    {
        foreach (Rigidbody rb in oldEnemy.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
        Destroy(oldEnemy.GetComponent<CollisionHandler>());
        Destroy(oldEnemy.GetComponent<EnemyAI>());
        Destroy(oldEnemy.GetComponent<EnemyHealth>());
        Destroy(oldEnemy.GetComponentInChildren<Animator>());
        Destroy(oldEnemy.GetComponent<NavMeshAgent>());
        _ragdolls.Add(oldEnemy);
    }
    
    IEnumerator CleanRagdolls()
    {
        while (true)
        {
            if (_ragdolls.Count >= maxAmountOfRagdolls)
            {
                _ragdolls.RemoveAt(0);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
