using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    [SerializeField] private Loot loot;
    private GameObject _lootObject;
    [SerializeField] private Transform lootShowcaseTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform lootTransform;
    [SerializeField] private GameObject glowing;
    [SerializeField] private Transform lid;
    private Material _glowingMaterial;
    private bool _opened = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        _glowingMaterial = glowing.GetComponent<Renderer>().material;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_opened)
        {
            OpenChest();
            _opened = true;
        }
    }

    private void OpenChest()
    {
        loot = LootManager.Instance.GetLoot();
        _glowingMaterial.SetColor("_EmissionColor", loot.color * 10);
        _lootObject = Instantiate(loot.prefab, lootTransform.position, lootTransform.rotation);
        animator.SetBool("Chest Opened", true);
        StartCoroutine(LerpToPosition());
        StartCoroutine(LerpToRotation());
    }


    private IEnumerator LerpToPosition()
    {
        yield return new WaitForSeconds(0.8f);
        while (_lootObject.transform.position != lootShowcaseTransform.position)
        {
            _lootObject.transform.position = Vector3.Lerp(_lootObject.transform.position, lootShowcaseTransform.position, Time.deltaTime * 2);
            yield return null;
        }
    }

    private IEnumerator LerpToRotation()
    {
        yield return new WaitForSeconds(0.8f);
        while (_lootObject.transform.rotation != lootShowcaseTransform.rotation)
        {
            _lootObject.transform.rotation = Quaternion.Lerp(_lootObject.transform.rotation, lootShowcaseTransform.rotation, Time.deltaTime * 2);
            yield return null;
        }
    }

}
