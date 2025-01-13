using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int damage;
    private SphereCollider _sphereCollider;
    private GameObject _target;
    [SerializeField] private int speed;

    private void Start()
    {
        _target = GameManager.Instance.player.gameObject;
        _sphereCollider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, speed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Collider[] colliders = Physics.OverlapSphere(_sphereCollider.center, _sphereCollider.radius);
            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(damage / 2);
                    Destroy(gameObject);
                }
            }
        }
    }
}
