using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyAI
{
    private Vector3 _endPosition;


    protected override void StartPreparingIdle()
    {
        SearchForPosition();
    }
    protected override void HandlingIdle()
    {
        CheckForEndPosition();
    }

    private void CheckForEndPosition()
    {
        if (Vector3.Distance(transform.position, _endPosition) < 0.1f)
        {
            SearchForPosition();
        }
    }
    private void SearchForPosition()
    {
        Vector3 position = (transform.position - _player.position).normalized;
        _endPosition = transform.position + position;
        _agent.SetDestination(_endPosition);

        
    }
}
