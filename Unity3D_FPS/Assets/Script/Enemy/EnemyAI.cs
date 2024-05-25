using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1,Idle = 0, Wander, Chase, Attack}

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private EnemyState enemyState = EnemyState.None;

    [Header("Radius Variables")]
    [SerializeField]
    private float           chaseRadius;    
    [SerializeField]
    private float           attackRadius;
    [SerializeField]
    private float           setDirectionSpeed;


    private Transform       target;
    private NavMeshAgent    nav;
    private Animator        ani;
    private Status          status;
    private void Awake()
    {
        nav     = GetComponent<NavMeshAgent>();
        ani     = GetComponent<Animator>();
        status  = GetComponent<Status>();

        target = GameObject.FindWithTag("Player").transform;    
    }

    private void OnEnable()
    {
        ChangeState(EnemyState.Idle);
    }
    private void OnDisable()
    {
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }
    public void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return;

        StopCoroutine(enemyState.ToString());

        enemyState = newState;

        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {

        StartCoroutine(ChangeToWander());
        nav.SetDestination(transform.position);

        while (true)
        {
            CalculateDisToTargetAndSelectState();
            yield return null;
        }
    }

    private IEnumerator ChangeToWander()
    {
        Vector3 originPos = transform.position;
        Vector3 targetPos = target.position;

        float dis = Vector3.Distance(originPos, targetPos);

        if(dis > chaseRadius)
        {
            float randomTime = Random.Range(1, 4);

            yield return new WaitForSeconds(randomTime);

            ChangeState(EnemyState.Wander);
        }
    }

    private IEnumerator Wander()
    {
        Debug.Log("Wander");

        float time = 0;
        float limitTime = 5;

        nav.speed = status.WalkSpeed;


        float randomPos = Random.Range(-10 , 10);
        Vector3 origin = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 target = new Vector3(origin.x + randomPos , 0, origin.z + randomPos);

        ani.SetBool("IsWander", true);
        while(true)
        {
            time += Time.deltaTime;

            CalculateDisToTargetAndSelectState();

            origin = new Vector3(transform.position.x, 0, transform.position.z);
            nav.SetDestination(target);

            if((target - origin).sqrMagnitude < 0.01f || time >= limitTime)
            {
                ani.SetBool("IsWander", false);
                ChangeState(EnemyState.Idle);
            }
            yield return null;
        }
    }

    private void CalculateDisToTargetAndSelectState()
    {
        Vector3 originPos = transform.position;
        Vector3 targetPos = target.position;

        float dis = Vector3.Distance(originPos, targetPos);

        if (dis <= chaseRadius && dis > attackRadius && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            ChangeState(EnemyState.Chase);
        }
        else if(dis <= attackRadius && !ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            ChangeState(EnemyState.Attack);
        }
        else
        {
            ani.SetBool("IsChase", false);
            ChangeState(EnemyState.Idle);
        }
    }

    private IEnumerator Chase()
    {
        ani.SetBool("IsWander", false);
        ani.SetBool("IsChase", true);

        while (true)
        {
            nav.speed = status.RunSpeed;

            nav.SetDestination(target.position);

            CalculateDisToTargetAndSelectState();

            yield return null;
        }
    }
    private IEnumerator Attack()
    {
        ani.SetBool("IsChase", false);
        while (true)
        {
            ani.SetTrigger("IsAttack");

            Vector3     dir     = (target.position - transform.position).normalized;
            Quaternion  lookRot = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * setDirectionSpeed);

            CalculateDisToTargetAndSelectState();

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
