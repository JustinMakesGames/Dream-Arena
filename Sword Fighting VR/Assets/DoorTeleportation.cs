using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DoorTeleportation : MonoBehaviour
{
    private Transform _outDoor;
    private void Start()
    {      
        StartCoroutine(WaitUntilObject());
    }

    private IEnumerator WaitUntilObject()
    {
        while (_outDoor == null)
        {
            if (GameObject.FindGameObjectWithTag("OutDoor") != null)
            {
                _outDoor = GameObject.FindGameObjectWithTag("OutDoor").transform;
            }
            
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Touched door");
            other.transform.position = _outDoor.position + _outDoor.forward;
            other.transform.rotation = _outDoor.rotation;
            Destroy(_outDoor.gameObject);
            Destroy(transform.root.gameObject);
        }
    }
}
