using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickUpWeapon : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.TryGetComponent<HandleGrabbing>(out _))
        {
            args.interactorObject.transform.GetComponent<HandleGrabbing>().AttachObject(transform);
        }
        LayerMask layerToIgnore = args.interactorObject.transform.gameObject.layer;
        Physics.IgnoreLayerCollision(layerToIgnore, gameObject.layer, true);
        GetComponent<Weapon>().isEquipped = true;
        base.OnSelectEntered(args);
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.TryGetComponent<HandleGrabbing>(out _))
        {
            args.interactorObject.transform.GetComponent<HandleGrabbing>().RemoveObject(transform);
        }
        GetComponent<Weapon>().isEquipped = false;
        LayerMask layerToIgnore = args.interactorObject.transform.gameObject.layer;
        Physics.IgnoreLayerCollision(layerToIgnore, gameObject.layer, false);
        base.OnSelectExited(args);
    }




}
