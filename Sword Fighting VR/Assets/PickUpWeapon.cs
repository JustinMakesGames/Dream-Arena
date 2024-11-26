using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickUpWeapon : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        args.interactorObject.transform.GetComponent<HandleGrabbing>().AttachObject(transform);
        base.OnSelectEntered(args);
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        args.interactorObject.transform.GetComponent<HandleGrabbing>().RemoveObject(transform);
        base.OnSelectExited(args);
    }




}
