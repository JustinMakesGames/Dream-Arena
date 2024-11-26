using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGrabbing : MonoBehaviour
{
    public Transform handPosition;
    public Transform holdingThisObject;

    private float distanceObject;
    [SerializeField] private float maxDistance;
    private void Update()
    {
        CheckForDistance();
    }

    public void AttachObject(Transform obj)
    {
        holdingThisObject = obj;
    }

    public void RemoveObject(Transform obj)
    {
        holdingThisObject = null;
    }

    private void CheckForDistance()
    {
        float distanceHand = Vector3.Distance(handPosition.position, transform.position);

        if (holdingThisObject != null)
        {
            distanceObject = Vector3.Distance(holdingThisObject.position, transform.position);
        }
        

        if  (distanceHand > maxDistance || holdingThisObject != null && distanceObject > maxDistance)
        {
            handPosition.GetComponent<Rigidbody>().position = transform.position;

            if (holdingThisObject != null)
            {
                holdingThisObject.position = transform.position;
            }
            
        }
    }
}
