using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerReferenceScript : MonoBehaviour
{
    public static PlayerReferenceScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
