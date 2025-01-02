using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportation2 : MonoBehaviour
{
    [SerializeField] private Transform teleportPlace;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VRTeleportScript.TeleportVrPlayer(other.transform, teleportPlace.position);
        }
    }
}
