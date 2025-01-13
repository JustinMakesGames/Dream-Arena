using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

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

    public static void SpawnRagdoll(EnemyHealth enemy, GameObject oldEnemy)
    {
        GameObject ragdoll = Instantiate(enemy.ragdoll, enemy.transform.position, enemy.transform.rotation);
        foreach (Rigidbody rb in ragdoll.GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        ragdoll.transform.SetParent(null);
        print("Instantiated " + ragdoll.name);
        if (enemy.hasLostLimb)
        {
            foreach (GameObject lostLimb in enemy.lostLimbs)
            {
                Destroy(ragdoll.transform.GetChild(0).gameObject.GetNamedChild(lostLimb.name));
            }
        }
        _ragdolls.Add(ragdoll);
        Destroy(oldEnemy);
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
