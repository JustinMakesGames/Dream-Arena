using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bow : Weapon
{
    [SerializeField] private Transform bowString;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;

    protected override void Start()
    {
        base.Start();
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = colliderMesh;
    }
    public void Shoot()
    {
        throw new NotImplementedException();
    }
}
