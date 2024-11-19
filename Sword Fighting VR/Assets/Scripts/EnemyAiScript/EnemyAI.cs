using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Idle Variables")]

    [SerializeField] private float range;
    [SerializeField] private EnemyStates enemyState;
    [SerializeField] private float intervalTime;
    private Vector3 _endDestination;
    private NavMeshAgent _agent;
    private Vector3 _originalPosition;

    [Header("Player Spotted")]
    [SerializeField] private Transform player;
    [SerializeField] private float enemyPositionOffset;
    [SerializeField] private float lookRange;
    public enum EnemyStates
    {
        Idle,
        PlayerSpotted,
        PlayerAttack
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _originalPosition = transform.position;
        Vector3 epicGaming = CalculateTargetPosition();
        _agent.SetDestination(epicGaming);
        
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyStates.Idle:
                HandlingIdle();
                break;
            case EnemyStates.PlayerSpotted:

                break;
        }
    }

    //Idle Mode

    private void HandlingIdle()
    {
        SearchForNewPosition();
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(player.position, transform.position) < lookRange)
        {
            HandlePlayerSpotted();
            enemyState = EnemyStates.PlayerSpotted;
        }
    }

    private void SearchForNewPosition()
    {
        if (Vector3.Distance(transform.position, _endDestination) < 0.1f)
        {
            _endDestination = GetPosition();
            StartCoroutine(WaitForNewPosition());
        }   
    }

    private IEnumerator WaitForNewPosition()
    {
        yield return new WaitForSeconds(intervalTime);
        _agent.SetDestination(_endDestination);
    }
    private Vector3 GetPosition()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(_originalPosition.x - range, _originalPosition.x + range),
            transform.position.y,
            Random.Range(_originalPosition.z - range, _originalPosition.z + range));

        return randomPos;
    }

    //Player spotted

    private void HandlePlayerSpotted()
    {
        OnTick.Instance.onTickEvent += MoveToThePlayer;
    }

    private void MoveToThePlayer()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        _agent.SetDestination(targetPosition);
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 endPosition = player.position + player.forward * enemyPositionOffset;
        return endPosition;
    }

    //Player Attacking
    protected virtual void HandleAttacking()
    {

    } 
    
}
