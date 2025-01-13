using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject fireball;
    public bool isAlive;

    private void Start()
    {
        StartCoroutine(AttackPattern());
    }

    private void Update()
    {
        if (!isAlive)
        {
            GameManager.Instance.HandleWinning();
        }
    }

    private IEnumerator AttackPattern()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            Instantiate(fireball, transform.position, transform.rotation);
        }
    }
}
