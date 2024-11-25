using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class GrabInteractable : XRGrabInteractable
{
    private Collider _collider;
    private LayerMask _layerToIgnore;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _layerToIgnore = LayerMask.GetMask("Ignore this layer");
    }

    //This makes sure when you grab something it won't start colliding with your hands.
    protected override void Grab()
    {
        base.Grab();
        var grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable.isSelected)
        {
            XRBaseInteractor interactor = grabInteractable.firstInteractorSelecting as XRBaseInteractor;
            if (interactor != null)
            {
                _layerToIgnore = interactor.gameObject.layer;
            }
        }
        Physics.IgnoreLayerCollision(_layerToIgnore, gameObject.layer);
    }

    //This makes sure when you drop something it doesn't immediately start colliding with your hands.
    protected override void Drop()
    {
        base.Drop();
        StartCoroutine(WaitForDrop(_layerToIgnore));
    }

    private IEnumerator WaitForDrop(LayerMask mask)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(mask, gameObject.layer, false);

    }
}
