using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idel,
    Wandering,
    Attacking
}

public class Monster : MonoBehaviour, IDamagable
{
    [Header("stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;

    public float attackAngle = 90f;

    public float lastAttackTime;
    public float attackDistance;

    private float playerDistance;
    public float fieldOfView = 100f;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    public event System.Action OnDeath;

    public bool forceChase = false; //레이드로 소환되는 좀비들

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        SetState(AIState.Wandering);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idel);

        switch (aiState)
        {
            case AIState.Idel:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        aiState = state;
        switch (aiState)
        {
            case AIState.Idel:
                agent.speed = walkSpeed;
                agent.isStopped = true;

                animator.SetFloat("IdleType", Random.value < 0.5f ? 0f : 1f);
                break;

            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;

            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()//평소에 돌아다니거나 배회하는 상태
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idel);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
        if (forceChase || playerDistance < detectDistance && IsPlayerInFieldOfView())
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idel) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLoction());
    }

    Vector3 GetWanderLoction()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }
        return hit.position;
    }

    void AttackingUpdate()
    {
        if (forceChase) //forceChase가 true인 동안에는 
        {
            agent.isStopped = false; //근접 공격 로직을 제외한 모든 조건을 무시하고
            agent.SetDestination(CharacterManager.Instance.Player.transform.position); //플레이어를 추격
            return; //아래 로직 무식
        }
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (playerDistance < detectDistance)
            {

                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, new NavMeshPath()))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            //드랍 아이템 설정
        }

        animator.SetBool("Death", true);

        agent.isStopped = true; //네브메쉬를 꺼놓음으로써 죽어도 플레이어를 따라가는 것을 방지함

        GetComponent<Collider>().enabled = false;
        OnDeath?.Invoke();
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void StopMoving()
    {
        agent.isStopped = false;
    }

    public void ApplyConeAttack()
    {
        Transform player = CharacterManager.Instance.Player.transform;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance) return; //부채꼴 거리? 공격 사거리 밖에 있으면 함수 종료

        Vector3 toPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > attackAngle * 0.5f) return; //부채꼴 각도 밖에 있으면 함수 종료

        IDamagable dmg = player.GetComponentInParent<IDamagable>();
        if (dmg != null)
            dmg.TakePhysicalDamage(damage);
    }
}
