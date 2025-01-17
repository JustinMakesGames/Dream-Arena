using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boomerang : Weapon
{
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }
    
    public override int GetDamage(Collider collision)
    {
        return base.GetDamage();
    }
    
    public void OnThrow()
    {
        Vector3 destination = transform.position + 
                              (transform.forward * rb.velocity.magnitude * 20);
        StartCoroutine(Throw(transform.position, destination));
    }

    IEnumerator Throw(Vector3 origin, Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(transform.position, destination, 
                Time.deltaTime * rb.velocity.magnitude);
            yield return null;
        }
        while (transform.position != origin)
        {
            transform.position = Vector3.Lerp(transform.position, destination, 
                Time.deltaTime * rb.velocity.magnitude);
            yield return null;
        }
        
    }
}
