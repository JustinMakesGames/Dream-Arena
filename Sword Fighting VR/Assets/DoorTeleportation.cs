using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportation : MonoBehaviour
{
    private Transform outDoor;
    private void Start()
    {      
        outDoor = GameObject.FindGameObjectWithTag("OutDoor").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Touched door");
            other.transform.position = outDoor.position + outDoor.forward;
            other.transform.rotation = outDoor.rotation;
            Destroy(outDoor.gameObject);
            Destroy(transform.root.gameObject);
        }
    }
}
