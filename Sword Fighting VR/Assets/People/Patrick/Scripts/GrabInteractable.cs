using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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
        Physics.IgnoreLayerCollision(gameObject.layer, GetLayerMask(), true);
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = false;
        }
    }
    private LayerMask GetLayerMask()
    {
        if (isSelected)
        {
            XRBaseInteractor interactor = firstInteractorSelecting as XRBaseInteractor;
            if (interactor != null)
            {
                return interactor.gameObject.layer;
                
            }
        }
        return LayerMask.GetMask("Ignore this layer");
    }
    //This makes sure when you drop something it doesn't immediately start colliding with your hands.
    protected override void Drop()
    {
        base.Drop();
        StartCoroutine(WaitForDrop(_layerMask));
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = true;
        }
    }

    private IEnumerator WaitForDrop(LayerMask mask)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(mask, gameObject.layer, false);

    }
}
