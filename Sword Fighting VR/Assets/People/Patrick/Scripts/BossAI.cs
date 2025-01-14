using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BossAI : MonoBehaviour
{
    private bool _test = true;
    private GameObject _realMrBeast;

    private void Start()
    {
        if (_test is not false)
        {
            Debug.Log("Test");
        }
    }
}
