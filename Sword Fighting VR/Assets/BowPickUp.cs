using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowPickUp : PickUpWeapon {
    private bool _currentlyPickedUp;
    [SerializeField] private int leftHand;
    [SerializeField] private int rightHand;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!_currentlyPickedUp)
        {
            base.OnSelectEntered(args);
            if (args.interactorObject.transform.gameObject.layer == rightHand)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, leftHand, true);
            }
            else if (args.interactorObject.transform.gameObject.layer == leftHand)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, rightHand, true);
            }
        }
        _currentlyPickedUp = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        
        if (_currentlyPickedUp)
        {
            base.OnSelectExited(args);
            if (args.interactorObject.transform.gameObject.layer == rightHand)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, leftHand, true);
            }
            else if (args.interactorObject.transform.gameObject.layer == leftHand)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, rightHand, true);
            }
        }
        _currentlyPickedUp = false;
    }

}
