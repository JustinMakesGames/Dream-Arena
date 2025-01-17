using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

public enum ColliderType {
    HEAD,
    BODY,
    ARM,
    LEG
}

public class EnemyHealth : MonoBehaviour
{
    //We don't want scripts to be able to edit the health directly,
    //so we're going to make it private but make sure it's still readable.
    private int _health;
    
    
    public int GetHealth() => _health;
    private DamageModifiers _damageModifiers;
    [SerializeField] private EnemyStats stats;
    public GameObject ragdoll;
    public bool hasLostLimb;
    public List<GameObject> lostLimbs; 
    public List<GameObject> losableLimbs = new();
    private GameObject _limbParent;
    private bool _dead = false;
    [SerializeField] private GameObject bloodParticles;
    private void Start()
    {
        if (transform.GetComponent<EnemyAI>() != null)
        {
            _limbParent = transform.GetChild(0).GetChild(0).gameObject;
            _limbParent.gameObject.GetChildGameObjects(losableLimbs);
            _damageModifiers = stats.damageModifiers;
            _health = stats.health;
        }
        
    }

    public void CalculateDamage(ColliderType colType, bool hasArmor, float armorDamageReduction, int baseDamage, bool ignoresArmor)
    {
        //When you hit the head we want it to do more damage than if you hit the arms,
        //so this is going to add a modifier depending on where you hit.
        int damage = Mathf.RoundToInt(colType switch
        {
            ColliderType.HEAD => baseDamage * _damageModifiers.headModifier,
            ColliderType.BODY => baseDamage * _damageModifiers.bodyModifier,
            ColliderType.LEG => baseDamage * _damageModifiers.legModifier,
            ColliderType.ARM => baseDamage * _damageModifiers.armModifier,
            _ => baseDamage
        });
        //Taking armor into account however how armor works in the future is subject to change.
        if (hasArmor && !ignoresArmor) damage = Mathf.RoundToInt(damage * armorDamageReduction);
        TakeDamage(damage, transform.position);
    }
#if UNITY_EDITOR
    [ContextMenu("Take Damage")]
    private void ContextMenuDamage()
    {
        TakeDamage(10000, transform.position);
    }
    #endif
    public void TakeDamage(int damage, Vector3 bloodPosition)
    {
        print(transform.name + " has been damaged");
        _health -= damage;
        GameObject bloodClone = Instantiate(bloodParticles, bloodPosition, Quaternion.identity);
        Destroy(bloodClone, 1f);
        if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().JumpFromDamage();
        if (_health <= 0 && !_dead)
        {
            if (GetComponentInChildren<Boss>() != null)
            {
                Destroy(gameObject);
            }

            else
            {
                _dead = true;
                if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().ChanceToSpawnItems();
                LoseRandomLimb();
                RagdollManager.SpawnRagdoll(gameObject);
            }
            
        }
    }
    [ContextMenu("Lose random limb")]
    public void LoseRandomLimb()
    {
        if (transform.GetComponent<EnemyAI>() == null) return;
        var limbToLose = losableLimbs[UnityEngine.Random.Range(0, losableLimbs.Count)];
        print(limbToLose.name);
        LoseLimb(limbToLose);
    }

    public void LoseLimb(GameObject limb)
    {
        if (transform.GetComponent<EnemyAI>() == null) return;
        print(limb.name);
        limb.GetComponent<SkinnedMeshRenderer>().rootBone = null;
        limb.GetComponent<SkinnedMeshRenderer>().bones = null;
        _limbParent.GetNamedChild(limb.name).transform.SetParent(null);
        lostLimbs.Add(limb);
        print(lostLimbs.Count);
        //Destroy related armor piece
        Destroy(gameObject.transform.GetChild(0).gameObject.GetNamedChild(limb.name));
        foreach (var go in lostLimbs)
        {
            print(go.name);
        }
        TakeDamage(_health, transform.position);
    } 
}
