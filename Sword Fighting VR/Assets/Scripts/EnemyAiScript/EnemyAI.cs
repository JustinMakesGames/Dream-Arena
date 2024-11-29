using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Idle Variables")]

    [SerializeField] private float range;
    public EnemyStates enemyState;
    [SerializeField] private float intervalTime;
    private Vector3 _endDestination;
    private NavMeshAgent _agent;
    private Vector3 _originalPosition;
    private bool hasFound;
    private bool hasMoved;

    [Header("Player Spotted")]
    private Transform _player;
    [SerializeField] private float enemyPositionOffset;
    [SerializeField] private float lookRange;

    [SerializeField] private float playerRange;

    [Header("Player Attacking")]
    [SerializeField] private float plusOffset;
    private Vector3 _newAttackDestination;
    private SwordBehaviour _swordBehaviourScript;
    public float changeAngle;
    public bool isAttacking;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce;
    private Rigidbody _rb;

    public EnemyStats enemyStats;
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
        _swordBehaviourScript = GetComponentInChildren<SwordBehaviour>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = PlayerReferenceScript.Instance.transform;
        SwitchOnce();
    }

    private void Update()
    {
        CheckForAttackDistance();
        switch (enemyState)
        {
            case EnemyStates.Idle:
                HandlingIdle();
                break;
            case EnemyStates.PlayerSpotted:

                break;
            case EnemyStates.PlayerAttack:
                HandlePlayerAttacking();
                break;
        }
    }

    private void SwitchOnce()
    {
        _agent.speed = 5f;
        switch (enemyState)
        {
            case EnemyStates.Idle:
                StartPreparingIdle();
                break;
            case EnemyStates.PlayerSpotted:
                StartPlayerSpotted();
                break;

            case EnemyStates.PlayerAttack:
                StartPreparingAttack();
                break;


        }
    }

    #region Idle

    private void StartPreparingIdle()
    {
        UnSubscribingFunctions();
        _endDestination = GetPosition();
        _agent.SetDestination(_endDestination);
    }

    private void UnSubscribingFunctions()
    {
        OnTick.Instance.onTickEvent -= MoveToThePlayer;
            
    }
    private void HandlingIdle()
    {
        SearchForNewPosition();
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(_player.position, transform.position) < lookRange)
        {
            enemyState = EnemyStates.PlayerSpotted;
            SwitchOnce();
        }
    }

    private void SearchForNewPosition()
    {
        if (Vector3.Distance(transform.position, _endDestination) < 0.5f && !hasMoved)
        {
            _endDestination = GetPosition();
            hasMoved = true;
            StartCoroutine(WaitForNewPosition());
        }   
    }

    private IEnumerator WaitForNewPosition()
    {
        yield return new WaitForSeconds(intervalTime);
        _agent.SetDestination(_endDestination);
        hasMoved = false;
    }
    private Vector3 GetPosition()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(_originalPosition.x - range, _originalPosition.x + range),
            transform.position.y,
            Random.Range(_originalPosition.z - range, _originalPosition.z + range));

        return randomPos;
    }
    #endregion

    #region PlayerSpotted

    private void StartPlayerSpotted()
    {
        OnTick.Instance.onTickEvent += MoveToThePlayer;
    }

    private void MoveToThePlayer()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        _agent.SetDestination(targetPosition);

        if (Vector3.Distance(targetPosition, transform.position) < 0.3f)
        {
            enemyState = EnemyStates.PlayerAttack;
            SwitchOnce();
        } 
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 directionToPlayer = _player.position - transform.position;
        directionToPlayer.Normalize();
        Vector3 endPosition = _player.position - directionToPlayer * enemyPositionOffset;
        return endPosition;
    }

    #endregion

    #region PlayerAttack

    private void StartPreparingAttack()
    {
        NavMeshAgentStatsChange();
        
        changeAngle = CalculateFirstPosition();
        _newAttackDestination = CalculatePositionToGo();
        _agent.SetDestination(_newAttackDestination);
        UnSubscribingFunctions();
    }

    private void NavMeshAgentStatsChange()
    {
        _agent.updateRotation = false;
        _agent.speed = 2;
    }

    protected virtual void HandlePlayerAttacking()
    {
        
        MoveAroundPlayer();
        CheckIfPlayerIsGone();
    }

    private void MoveAroundPlayer()
    {
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
        if (Vector3.Distance(transform.position, _newAttackDestination) == 0)
        {
            _newAttackDestination = CalculatePositionToGo();
            _agent.SetDestination(_newAttackDestination);
        }
    }

    private void CheckIfPlayerIsGone()
    {
        if (Vector3.Distance(_player.position, transform.position) > playerRange)
        {
            enemyState = EnemyStates.PlayerSpotted;
            _agent.updateRotation = true;
            MoveToThePlayer();
            SwitchOnce();
        }
    }

    private float CalculateFirstPosition()
    {
        Vector3 direction = (transform.position - _player.position).normalized;

        direction.y = 0;
        float angleFromPlayer  = Vector3.SignedAngle(_player.position, direction, Vector3.up);

        return angleFromPlayer;
    }

    private Vector3 CalculatePositionToGo()
    {
        changeAngle += Random.Range(-20, 20);
        float radians = changeAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(_player.position.x + Mathf.Cos(radians) * plusOffset, transform.position.y, _player.position.z + Mathf.Sin(radians) * plusOffset);
        return offset;
    }

    private void CheckForAttackDistance()
    {
        if (Vector3.Distance(transform.position, _player.position) < playerRange && !isAttacking)
        {
            isAttacking = true;
            _swordBehaviourScript.StartAttacking();
            
        }

        else if (Vector3.Distance(transform.position, _player.position) > playerRange && isAttacking)
        {
            isAttacking = false;
        }
    }
    #endregion

}
