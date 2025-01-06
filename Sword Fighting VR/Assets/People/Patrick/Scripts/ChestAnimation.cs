using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    public bool opened { get; private set; }
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        _glowingMaterial = glowing.GetComponent<Renderer>().material;
    }

    

    public void OpenChest()
    {
        opened = true;
        loot = LootManager.Instance.GetLoot();
        _glowingMaterial.SetColor("_EmissionColor", loot.color * 10);
        _lootObject = Instantiate(loot.prefab, lootTransform.position, lootTransform.rotation);
        _lootObject.GetComponent<Rigidbody>().useGravity = false;
        animator.SetBool("Chest Opened", true);
        StartCoroutine(LerpToPosition());
        StartCoroutine(LerpToRotation());
    }


    private IEnumerator LerpToPosition()
    {
        yield return new WaitForSeconds(0.8f);
        while (Vector3.Distance(_lootObject.transform.position, lootShowcaseTransform.position) > 0.1f)
        {
            _lootObject.transform.position = Vector3.Lerp(_lootObject.transform.position, lootShowcaseTransform.position, Time.deltaTime * 2);
            yield return null;
        }
    }

    private IEnumerator LerpToRotation()
    {
        yield return new WaitForSeconds(0.8f);
        while (Vector3.Distance(_lootObject.transform.rotation.eulerAngles, lootShowcaseTransform.rotation.eulerAngles) > 0.1f)
        {
            _lootObject.transform.rotation = Quaternion.Lerp(_lootObject.transform.rotation, lootShowcaseTransform.rotation, Time.deltaTime * 2);
            yield return null;
        }
    }

}
