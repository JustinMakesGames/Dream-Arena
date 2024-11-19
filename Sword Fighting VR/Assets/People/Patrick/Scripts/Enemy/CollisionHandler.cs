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
            _damage = other.gameObject.GetComponent<Weapon>().GetDamage();
            transform.root.GetComponent<Health>().CalculateDamage(type, hasArmor, armorDamageReduction, _damage);
            StartCoroutine(ChangeColor());
        }
    }

    IEnumerator ChangeColor()
    {
        transform.root.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        transform.root.GetComponent<MeshRenderer>().material.color = Color.white;

    }
}
