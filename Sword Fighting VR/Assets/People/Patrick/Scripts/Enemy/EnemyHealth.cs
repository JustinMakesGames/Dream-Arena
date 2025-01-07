using System;
using Unity.XR.CoreUtils;
using UnityEngine;
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
    public GameObject[] lostLimbs;
    private void Start()
    {
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

    [ContextMenu("Take Damage")]
    private void ContextMenuDamage()
    {
        TakeDamage(10000);
    }
    
    private void TakeDamage(int damage)
    {
        _health -= damage;

        GetComponent<EnemyAI>().JumpFromDamage();
        if (_health <= 0)
        {
            GetComponent<EnemyAI>().ChanceToSpawnItems();
            Destroy(gameObject);
            //RagdollManager.SpawnRagdoll(this);
        }
    }

    public void LoseLimb(GameObject limb)
    {
        gameObject.GetNamedChild(limb.name).transform.SetParent(GameObject.Find("LostLimbs").transform);
        TakeDamage(_health);
    } 
}
