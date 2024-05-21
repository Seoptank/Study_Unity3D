using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None, Idle, Wander ,Chase}

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float               chaseDis;
    [SerializeField]
    private Transform           playerTransform;

    private NavMeshAgent        navMeshAgent;
    private PlayerController    playerController;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CalculateDistanceToTarget();
    }

    private void CalculateDistanceToTarget()
    {
        Vector3 originPos = transform.position;
        Vector3 targetPos = playerTransform.position;

        float dis = Vector3.Distance(originPos, targetPos);

        if(dis <= chaseDis)
        {
            navMeshAgent.SetDestination(targetPos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDis);
    }
}
