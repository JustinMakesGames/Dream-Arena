using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Idle Variables")]

    [SerializeField] protected float range;
    public EnemyBehaviourStates enemyState;
    [SerializeField] protected float intervalTime;
    protected Vector3 _endDestination;
    protected NavMeshAgent _agent;
    protected Vector3 _originalPosition;
    protected bool _hasFound;
    protected bool _hasMoved;

    [Header("Player Spotted")]
    protected Transform _player;
    [SerializeField] protected float enemyPositionOffset;
    [SerializeField] protected float lookRange;

    [SerializeField] protected float playerRange;

    [Header("Player Attacking")]
    [SerializeField] protected float plusOffset;
    protected Vector3 _newAttackDestination;
    protected SwordBehaviour _swordBehaviourScript;
    public float changeAngle;
    public bool isAttacking;

    [Header("Knockback")]
    [SerializeField] protected float knockbackForce;
    protected Rigidbody _rb;

    public EnemyStats enemyStats;

    [SerializeField] protected float jumpForce;
    [SerializeField] protected float knockbackDuration;

    [Header("DeathSpawning")]
    [SerializeField] protected GameObject item;

    public enum EnemyBehaviourStates
    {
        Idle,
        PlayerSpotted,
        PlayerAttack
    }

    protected void Awake()
    {
        
        _agent = GetComponent<NavMeshAgent>();
        _originalPosition = transform.position;
        _swordBehaviourScript = GetComponentInChildren<SwordBehaviour>();
        _rb = GetComponent<Rigidbody>();
    }

    protected void Start()
    {
        _player = PlayerReferenceScript.Instance.transform;
        SwitchOnce();
    }

    protected void Update()
    {
        CheckForAttackDistance();
        switch (enemyState)
        {
            case EnemyBehaviourStates.Idle:
                HandlingIdle();
                break;
            case EnemyBehaviourStates.PlayerSpotted:

                break;
            case EnemyBehaviourStates.PlayerAttack:
                HandlePlayerAttacking();
                break;
        }
    }

    protected void SwitchOnce()
    {
        _agent.speed = 5f;
        switch (enemyState)
        {
            case EnemyBehaviourStates.Idle:
                StartPreparingIdle();
                break;
            case EnemyBehaviourStates.PlayerSpotted:
                StartPlayerSpotted();
                break;

            case EnemyBehaviourStates.PlayerAttack:
                StartPreparingAttack();
                break;


        }
    }

    #region Idle

    protected virtual void StartPreparingIdle()
    {
        UnsubscribeFromOnTick();
        _endDestination = GetPosition();
        _agent.SetDestination(_endDestination);
    }

    protected void UnsubscribeFromOnTick()
    {
        OnTick.Instance.onTickEvent -= MoveToThePlayer;
            
    }
    protected virtual void HandlingIdle()
    {
        SearchForNewPosition();
        CheckForPlayer();
    }

    protected void CheckForPlayer()
    {
        if (Vector3.Distance(_player.position, transform.position) < lookRange)
        {
            enemyState = EnemyBehaviourStates.PlayerSpotted;
            SwitchOnce();
        }
    }

    protected void SearchForNewPosition()
    {
        if (Vector3.Distance(transform.position, _endDestination) < 0.5f && !_hasMoved)
        {
            _endDestination = GetPosition();
            _hasMoved = true;
            StartCoroutine(WaitForNewPosition());
        }   
    }

    protected IEnumerator WaitForNewPosition()
    {
        yield return new WaitForSeconds(intervalTime);
        _agent.SetDestination(_endDestination);
        _hasMoved = false;
    }
    protected Vector3 GetPosition()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(_originalPosition.x - range, _originalPosition.x + range),
            transform.position.y,
            Random.Range(_originalPosition.z - range, _originalPosition.z + range));

        return randomPos;
    }
    #endregion

    #region PlayerSpotted

    protected void StartPlayerSpotted()
    {
        OnTick.Instance.onTickEvent += MoveToThePlayer;
    }

    protected void MoveToThePlayer()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        _agent.SetDestination(targetPosition);

        if (Vector3.Distance(targetPosition, transform.position) < 0.3f)
        {
            enemyState = EnemyBehaviourStates.PlayerAttack;
            SwitchOnce();
        } 
    }

    protected Vector3 CalculateTargetPosition()
    {
        Vector3 directionToPlayer = _player.position - transform.position;
        directionToPlayer.Normalize();
        Vector3 endPosition = _player.position - directionToPlayer * enemyPositionOffset;
        return endPosition;
    }

    #endregion

    #region PlayerAttack

    protected virtual void StartPreparingAttack()
    {
        NavMeshAgentStatsChange();
        
        changeAngle = CalculateFirstPosition();
        _newAttackDestination = CalculatePositionToGo();
        _agent.SetDestination(_newAttackDestination);
        UnsubscribeFromOnTick();
    }

    protected void NavMeshAgentStatsChange()
    {
        _agent.updateRotation = false;
        _agent.speed = 2;
    }

    protected virtual void HandlePlayerAttacking()
    {
        
        MoveAroundPlayer();
        CheckIfPlayerIsGone();
    }

    protected void MoveAroundPlayer()
    {
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
        if (Vector3.Distance(transform.position, _newAttackDestination) == 0)
        {
            _newAttackDestination = CalculatePositionToGo();
            _agent.SetDestination(_newAttackDestination);
        }
    }

    protected void CheckIfPlayerIsGone()
    {
        if (Vector3.Distance(_player.position, transform.position) > playerRange)
        {
            enemyState = EnemyBehaviourStates.PlayerSpotted;
            _agent.updateRotation = true;
            MoveToThePlayer();
            SwitchOnce();
        }
    }

    protected float CalculateFirstPosition()
    {
        Vector3 direction = (transform.position - _player.position).normalized;

        direction.y = 0;
        float angleFromPlayer  = Vector3.SignedAngle(_player.position, direction, Vector3.up);

        return angleFromPlayer;
    }

    protected Vector3 CalculatePositionToGo()
    {
        changeAngle += Random.Range(-20, 20);
        float radians = changeAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(_player.position.x + Mathf.Cos(radians) * plusOffset, transform.position.y, _player.position.z + Mathf.Sin(radians) * plusOffset);
        return offset;
    }

    protected void CheckForAttackDistance()
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
    protected void OnDestroy()
    {
        OnTick.Instance.onTickEvent -= MoveToThePlayer;    
    }

    public void JumpFromDamage()
    {
        _agent.enabled = false;
        Vector3 direction = (transform.position - _player.position).normalized;
        direction.y = jumpForce;
        print("jumped");
        _rb.isKinematic = false;
        _rb.AddForce(direction * knockbackForce, ForceMode.VelocityChange);
        print(direction * knockbackForce);

        Invoke(nameof(ResetKnockback), knockbackDuration);

        
    }

    protected void ResetKnockback()
    {
        _rb.velocity = Vector3.zero;
        _agent.enabled = true;
        _rb.isKinematic = true;
    }

    public void ChanceToSpawnItems()
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }

}
