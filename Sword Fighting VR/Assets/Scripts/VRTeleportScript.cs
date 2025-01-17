using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRTeleportScript
{
    public static void TeleportVrPlayer(Transform player, Vector3 endPosition)
    {
        player.position = endPosition;
        player.parent.GetChild(1).position = endPosition;
        player.parent.GetChild(2).position = endPosition;

        if (player.parent.childCount  >= 4)
        {
            player.parent.GetChild(3).position = endPosition;
        }
    }

    public static void SetRotation(Transform player, Quaternion rotation)
    {
        player.rotation = rotation;
        player.parent.GetChild(1).rotation = rotation;
        player.parent.GetChild(2).rotation = rotation;

        if (player.parent.childCount >= 4)
        {
            player.parent.GetChild(3).rotation = rotation;
        }
    }
}
