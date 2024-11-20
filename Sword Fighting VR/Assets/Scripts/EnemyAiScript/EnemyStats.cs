using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType {
    MELEE,
    RANGED
}

[Serializable]
public struct DamageModifiers
{
    public float headModifier;
    public float bodyModifier;
    public float armModifier;
    public float legModifier;
}
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{
    
    public int damage;
    public int health;
    public EnemyType type;
    public DamageModifiers DamageModifiers;
}
