using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringInteractable : GrabInteractable
{
    private Vector3 _oldTransform;
    private Quaternion _baseRotation;
    [SerializeField] private Bow bow;
    [SerializeField] private GameObject arrowPlaceHolder;

    void Start()
    {
        _baseRotation = arrowPlaceHolder.transform.localRotation;
        arrowPlaceHolder.SetActive(false);
    }
    protected override void Grab()
    {
        arrowPlaceHolder.SetActive(true);
        _oldTransform = transform.localPosition;
    }
    
    protected override void Drop()
    {
        print("Dropped");
        float speed = Vector3.Distance(_oldTransform, transform.localPosition) * 10; 
        StartCoroutine(MoveStringBack());
        arrowPlaceHolder.SetActive(false);
        bow.Shoot(transform.GetChild(1).position, speed, arrowPlaceHolder.transform.localRotation);
        arrowPlaceHolder.transform.localRotation = _baseRotation;
        
    }

    IEnumerator MoveStringBack()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        while (transform.localPosition != _oldTransform)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _oldTransform, Time.deltaTime * 10);
            yield return null;
        }
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
