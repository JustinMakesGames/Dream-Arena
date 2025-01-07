using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowPhysics : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
