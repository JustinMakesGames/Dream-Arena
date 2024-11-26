using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimations : MonoBehaviour
{
    public InputActionProperty pinchAnimation;
    public InputActionProperty grabAnimation;
    public Animator handAnimator;

    public float pinchInput;
    public float grabInput;

    private void Update()
    {
        ControlHandAnimations();
    }

    private void ControlHandAnimations()
    {
        pinchInput = pinchAnimation.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", pinchInput);

        grabInput = grabAnimation.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", grabInput);
    }
}
