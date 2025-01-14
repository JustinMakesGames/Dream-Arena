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
    [SerializeField]
    private void Start()
    {
        _limbParent = transform.GetChild(0).GetChild(0).gameObject;
        _limbParent.gameObject.GetChildGameObjects(losableLimbs);
        _damageModifiers = stats.damageModifiers;
        _health = stats.health;
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
        TakeDamage(damage);
    }
#if UNITY_EDITOR
    [ContextMenu("Take Damage")]
    private void ContextMenuDamage()
    {
        TakeDamage(10000);
    }
    #endif
    public void TakeDamage(int damage)
    {
        _health -= damage;

        GetComponent<EnemyAI>().JumpFromDamage();
        if (_health <= 0 && !_dead)
        {
            _dead = true;
            GetComponent<EnemyAI>().ChanceToSpawnItems();
            LoseRandomLimb();
            RagdollManager.SpawnRagdoll(gameObject);
        }
    }
    [ContextMenu("Lose random limb")]
    public void LoseRandomLimb()
    {
        var limbToLose = losableLimbs[UnityEngine.Random.Range(0, losableLimbs.Count)];
        print(limbToLose.name);
        LoseLimb(limbToLose);
    }

    public void LoseLimb(GameObject limb)
    {
        print(limb.name);
        limb.GetComponent<SkinnedMeshRenderer>().rootBone = null;
        limb.GetComponent<SkinnedMeshRenderer>().bones = null;
        _limbParent.GetNamedChild(limb.name).transform.SetParent(null);
        lostLimbs.Add(limb);
        print(lostLimbs.Count);
        Destroy(gameObject.GetNamedChild(limb.name));
        foreach (var go in lostLimbs)
        {
            print(go.name);
        }
        TakeDamage(_health);
    } 
}
