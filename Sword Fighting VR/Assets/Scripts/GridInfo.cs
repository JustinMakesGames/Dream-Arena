using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    [SerializeField] private bool isWalkable;

    public void GetWalkableInfo()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.CompareTag("Ground"))
            {
                isWalkable = true;
            }
        }
    }
}
