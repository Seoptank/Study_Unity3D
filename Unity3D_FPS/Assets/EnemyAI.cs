using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1,Idle = 0, Wander, Chase}

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private EnemyState enemyState = EnemyState.None;

    [Header("Radius Variables")]
    [SerializeField]
    private float           chaseRadius;
    [SerializeField]
    private float           wanderRadius;

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

        while(true)
        {
            yield return null;
        }
    }

    private IEnumerator ChangeToWander()
    {
        float randomTime = Random.Range(1, 4);

        yield return new WaitForSeconds(randomTime);

        ChangeState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
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

            IsChase();

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

    private bool IsChase()
    {
        Vector3 originPos = transform.position;
        Vector3 targetPos = target.position;

        float dis = Vector3.Distance(originPos, targetPos);

        if (dis <= chaseRadius)
        {
            ani.SetBool("IsWander", false);
            ChangeState(EnemyState.Chase);
            return true;
        }
        else
        { 
            ChangeState(EnemyState.Idle);
            return false;
        }
    }

    private IEnumerator Chase()
    {
        ani.SetBool("IsChase", true);
        while (IsChase())
        {
            nav.speed = status.RunSpeed;

            nav.SetDestination(target.position);

            if(!IsChase())
            {
                nav.SetDestination(transform.position);
                ani.SetBool("IsChase", false);
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
