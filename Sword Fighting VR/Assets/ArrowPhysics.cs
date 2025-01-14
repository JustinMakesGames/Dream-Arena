using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowPhysics : MonoBehaviour
{
    public float speed;
    [SerializeField] private float destroyTime;
    [SerializeField] private WeaponSO weaponSo;

    private void Start()
    {
        Destroy(gameObject.transform.parent.gameObject, destroyTime);
    }
    private void Update()
    {
        transform.parent.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        
        if (other.TryGetComponent(out EnemyHealth health))
        {
            health.TakeDamage(weaponSo.damage);
        }
    }
}
