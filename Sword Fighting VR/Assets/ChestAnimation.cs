using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    [SerializeField] private GameObject loot;
    [SerializeField] private Transform lootShowcaseTransform;
    [SerializeField] private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OpenChest()
    {
        animator.Play(0);
        StartCoroutine(LerpToPosition());
        StartCoroutine(LerpToRotation());
    }


    private IEnumerator LerpToPosition()
    {
        yield return new WaitForSeconds(0.5f);
        while (loot.transform.position != lootShowcaseTransform.position)
        {
            loot.transform.position = Vector3.Lerp(loot.transform.position, lootShowcaseTransform.position, Time.deltaTime * 5);
            yield return null;
        }
    }

    private IEnumerator LerpToRotation()
    {
        yield return new WaitForSeconds(0.5f);
        while (loot.transform.rotation != lootShowcaseTransform.rotation)
        {
            loot.transform.rotation = Quaternion.Lerp(loot.transform.rotation, lootShowcaseTransform.rotation, Time.deltaTime * 5);
            yield return null;
        }
    }

}
