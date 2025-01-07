using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : EnemyAI
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float distance;
    private Vector3 _endPosition;

    private void CheckForEndPosition()
    {
        if (Vector3.Distance(transform.position, _endPosition) < 0.01f)
        {
            SearchForNewPosition();
        }
    }

    private void SearchForPosition()
    {
        Vector3 position = (transform.position - _player.position).normalized * distance;
        Vector3 estimatedPosition = transform.position + position;
        
        if (NavMesh.SamplePosition(estimatedPosition, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas)) 
        {
            _endPosition = hit.position;
            _endPosition.y++;

        }
        _agent.SetDestination(_endPosition);      
    }

    protected override void StartPreparingAttack()
    {
        isAttacking = true;
        SearchForPosition();
        StartCoroutine(SpawnArrow());
    }

    protected override void HandlePlayerAttacking()
    {
        if (!isAttacking)
        {
            StopCoroutine(SpawnArrow());
        }
    }

    private IEnumerator SpawnArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            Instantiate(arrow, transform.position, transform.rotation);
        }
    }






}
