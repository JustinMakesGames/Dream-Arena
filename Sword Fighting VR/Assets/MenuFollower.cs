using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFollower : MonoBehaviour
{
    [SerializeField] private float canvasDistance;
    private Transform cam;
    

    private void Start()
    {
        cam = Camera.main.transform;
    }
    private void Update()
    {
        transform.position = cam.position + new Vector3(cam.forward.x, transform.forward.y, cam.forward.z) * canvasDistance;

        transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
    }
}
