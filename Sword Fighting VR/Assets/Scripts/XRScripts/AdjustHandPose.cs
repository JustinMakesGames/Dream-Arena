using UnityEngine;
using UnityEngine.XR;

public class AdjustHandPose : MonoBehaviour
{
    public XRNode handNode; // LeftHand or RightHand
    public Vector3 positionOffset;

    void Update()
    {
        if (InputDevices.GetDeviceAtXRNode(handNode).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition))
        {
            transform.position = handPosition + positionOffset;
        }
    }
}
