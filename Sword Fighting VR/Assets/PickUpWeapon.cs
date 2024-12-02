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
        if (args.interactorObject.transform.TryGetComponent(out Weapon weapon)) weapon.isEquipped = true;
        base.OnSelectEntered(args);
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.TryGetComponent<HandleGrabbing>(out _))
        {
            args.interactorObject.transform.GetComponent<HandleGrabbing>().RemoveObject(transform);
        }
        if (args.interactorObject.transform.TryGetComponent(out Weapon weapon)) weapon.isEquipped = false;
        base.OnSelectExited(args);
    }




}
