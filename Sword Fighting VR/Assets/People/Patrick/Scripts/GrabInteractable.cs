using System;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(Collider))]
public class GrabInteractable : XRGrabInteractable
{
    private Collider _collider;
    private LayerMask _layerMask;
    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    //This makes sure when you grab something it won't start colliding with your hands.
    protected override void Grab()
    {
        base.Grab();
        Physics.IgnoreLayerCollision(GetLayerMask(), gameObject.layer);
    }

    private LayerMask GetLayerMask()
    {
        var grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable.isSelected)
        {
            XRBaseInteractor interactor = grabInteractable.firstInteractorSelecting as XRBaseInteractor;
            if (interactor != null)
            {
                _layerMask = interactor.gameObject.layer;
                return _layerMask;
            }
        }
        return LayerMask.GetMask("Ignore this layer");
    }
    //This makes sure when you drop something it doesn't immediately start colliding with your hands.
    protected override void Drop()
    {
        base.Drop();
        StartCoroutine(WaitForDrop(_layerMask));
    }

    private IEnumerator WaitForDrop(LayerMask mask)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(mask, gameObject.layer, false);

    }
}
