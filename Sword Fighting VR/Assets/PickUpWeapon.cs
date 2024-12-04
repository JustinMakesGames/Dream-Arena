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

        if (args.interactorObject.transform.name == "LeftRay")
        {
            args.interactorObject.transform.root.GetChild(1).GetComponent<WeaponManager>().SwitchWeapon(GetComponent<Weapon>());
        }

        else
        {
            args.interactorObject.transform.root.GetChild(2).GetComponent<WeaponManager>().SwitchWeapon(GetComponent<Weapon>());
        }
        
        LayerMask layerToIgnore = args.interactorObject.transform.gameObject.layer;
        Physics.IgnoreLayerCollision(layerToIgnore, gameObject.layer, true);
        GetComponent<Weapon>().isEquipped = true;
        base.OnSelectEntered(args);
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        print("Exited");
        if (args.interactorObject.transform.TryGetComponent<HandleGrabbing>(out _))
        {
            args.interactorObject.transform.GetComponent<HandleGrabbing>().RemoveObject(transform);
        }

        if (args.interactorObject.transform.name == "LeftRay")
        {
            args.interactorObject.transform.root.GetChild(1).GetComponent<WeaponManager>().DetachWeapon();
        }

        else
        {
            args.interactorObject.transform.root.GetChild(2).GetComponent<WeaponManager>().DetachWeapon();
        }
        GetComponent<Weapon>().isEquipped = false;
        LayerMask layerToIgnore = args.interactorObject.transform.gameObject.layer;
        Physics.IgnoreLayerCollision(layerToIgnore, gameObject.layer, false);
        base.OnSelectExited(args);
    }




}
