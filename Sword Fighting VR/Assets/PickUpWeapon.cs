using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickUpWeapon : XRGrabInteractable
{
    private LayerMask _layerMask;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.TryGetComponent(out HandleGrabbing component))
        {
            component.AttachObject(transform);
        }

        if (args.interactorObject.transform.name == "LeftRay")
        {
            args.interactorObject.transform.root.GetChild(1).GetComponent<WeaponManager>().SwitchWeapon(GetComponent<Weapon>());
            GetComponent<Weapon>().UpdateController(args.interactorObject.transform.root.GetChild(1));
        }

        else
        {
            args.interactorObject.transform.root.GetChild(2).GetComponent<WeaponManager>().SwitchWeapon(GetComponent<Weapon>());
            GetComponent<Weapon>().UpdateController(args.interactorObject.transform.root.GetChild(2));
        }
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Weapon>().isEquipped = true;
        base.OnSelectEntered(args);
        Physics.IgnoreLayerCollision(gameObject.layer, args.interactorObject.transform.gameObject.layer, true);

        transform.parent = args.interactorObject.transform.root;
        
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        print("Exited");
        if (args.interactorObject.transform.TryGetComponent(out HandleGrabbing handle))
        {
            handle.RemoveObject(transform);
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

        transform.parent = null;
        base.OnSelectExited(args);
        StartCoroutine(WaitForDrop(args.interactorObject.transform.gameObject.layer));
    }
    
    private IEnumerator WaitForDrop(LayerMask mask)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(mask, gameObject.layer, false);

    }
}
