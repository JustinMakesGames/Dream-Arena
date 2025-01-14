using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingHandler : MonoBehaviour
{
    public void HandleAttackAnimation()
    {
        transform.parent.GetComponent<EnemyAI>().HandleAttackAnimation();
    }
}
