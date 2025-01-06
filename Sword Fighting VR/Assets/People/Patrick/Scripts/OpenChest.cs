using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenChest : XRBaseInteractable
{
    private ChestAnimation _chest;
    private void Start()
    {
        _chest = GetComponent<ChestAnimation>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!_chest.opened)
        {
            _chest.OpenChest();
        }
    }
}
