using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ColliderType type;
    [SerializeField] private bool hasArmor;
    [SerializeField] private float armorDamageReduction;
    [SerializeField] private int headDamage, bodyDamage, legDamage, armDamage;
    private int _damage;
    private void OnCollisionEnter(Collision other)
    {
        _damage = other.gameObject.GetComponent<Weapon>().GetDamage();
        transform.parent.root.GetComponent<Health>().HandleCollisions(type, hasArmor, armorDamageReduction, _damage);
    }

    private int CalculateDamage(int baseDamage, ColliderType colType)
    {
        int damage = colType switch
        {
            ColliderType.HEAD => headDamage,
            ColliderType.BODY => bodyDamage,
            ColliderType.LEG => legDamage,
            ColliderType.ARM => armDamage,
            _ => 0
        };
        return damage;
    }
}
