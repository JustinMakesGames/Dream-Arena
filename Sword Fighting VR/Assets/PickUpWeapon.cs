using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickUpWeapon : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        GetComponent<Sword>().enabled = true;
        GetComponent<Sword>().SetHandLocation(args.interactableObject.transform);
        base.OnSelectEntered(args);
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        GetComponent<Sword>().enabled = false;
        GetComponent<Sword>().SetHandLocation(null);
        base.OnSelectExited(args);
    }




}
