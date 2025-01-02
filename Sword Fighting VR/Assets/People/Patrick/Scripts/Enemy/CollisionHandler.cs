using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ColliderType type;
    [SerializeField] private bool hasArmor;
    [SerializeField] private float armorDamageReduction;
    private int _damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();
            if (weapon.isEquipped)
            {
                _damage = weapon.GetDamage(other);
                bool ignoresArmor = weapon.IgnoresArmor();
                if (_damage == 0)
                {
                    return;
                }
                transform.root.GetComponent<EnemyHealth>().CalculateDamage(type, hasArmor, armorDamageReduction, _damage, ignoresArmor);
                StartCoroutine(ChangeColor());
            }
        }
    }
    
    IEnumerator ChangeColor()
    {
        Renderer[] renderers = transform.root.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.white;
        }

    }
}
