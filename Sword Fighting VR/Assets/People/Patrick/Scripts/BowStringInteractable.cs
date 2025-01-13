using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringInteractable : GrabInteractable
{
    private Vector3 _oldTransform;
    [SerializeField] private bool hasArrow;
    [SerializeField] private Bow bow;
    private GameObject _pijlPlaceHolder;

    void Start()
    {
        _pijlPlaceHolder = gameObject.GetNamedChild("PijlPlaceholder");
        _pijlPlaceHolder.SetActive(false);
    }
    protected override void Grab()
    {
        _pijlPlaceHolder.SetActive(true);
        _oldTransform = transform.localPosition;
    }
    
    protected override void Drop()
    {
        print("Dropped");
        float speed = Vector3.Distance(_oldTransform, transform.localPosition) * 10; 
        StartCoroutine(MoveStringBack());
        _pijlPlaceHolder.SetActive(false);
        bow.Shoot(transform.GetChild(0).position, speed);
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
