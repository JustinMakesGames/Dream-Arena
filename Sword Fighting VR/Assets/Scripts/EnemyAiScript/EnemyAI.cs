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

    [Header("Player Spotted")]
    [SerializeField] private Transform player;
    [SerializeField] private float enemyPositionOffset;
    [SerializeField] private float lookRange;

    [SerializeField] private float playerRange;

    [Header("Player Attacking")]
    [SerializeField] private float plusOffset;
    private Vector3 _newAttackDestination;
    private SwordBehaviour _swordBehaviourScript;
    public float changeAngle;
    public bool isAttacking;
    public enum EnemyStates : byte
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
    }

    private void Start()
    {
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
        _agent.speed = 10f;
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
        if (Vector3.Distance(player.position, transform.position) < lookRange)
        {
            enemyState = EnemyStates.PlayerSpotted;
            SwitchOnce();
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
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        Vector3 endPosition = player.position - directionToPlayer * enemyPositionOffset;
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
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (Vector3.Distance(transform.position, _newAttackDestination) == 0)
        {
            _newAttackDestination = CalculatePositionToGo();
            _agent.SetDestination(_newAttackDestination);
        }
    }

    private void CheckIfPlayerIsGone()
    {
        if (Vector3.Distance(player.position, transform.position) > playerRange)
        {
            enemyState = EnemyStates.PlayerSpotted;
            _agent.updateRotation = true;
            MoveToThePlayer();
            SwitchOnce();
        }
    }

    private float CalculateFirstPosition()
    {
        Vector3 direction = (transform.position - player.position).normalized;

        direction.y = 0;
        float angleFromPlayer  = Vector3.SignedAngle(player.position, direction, Vector3.up);

        return angleFromPlayer;
    }

    private Vector3 CalculatePositionToGo()
    {
        changeAngle += Random.Range(-20, 20);
        float radians = changeAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(player.position.x + Mathf.Cos(radians) * plusOffset, transform.position.y, player.position.z + Mathf.Sin(radians) * plusOffset);
        return offset;
    }

    private void CheckForAttackDistance()
    {
        if (Vector3.Distance(transform.position, player.position) < playerRange && !isAttacking)
        {
            isAttacking = true;
            _swordBehaviourScript.StartAttacking();
            
        }

        else if (Vector3.Distance(transform.position, player.position) > playerRange && isAttacking)
        {
            isAttacking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            print("Got hit");
        }
    }

    #endregion
}
