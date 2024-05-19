using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float radius;   
    [SerializeField]
    private float posY;                     // 생성항 
    [SerializeField]
    private float waitSec;                  // 대기 시간
    [SerializeField]
    private GameObject  ballPrefab;         // 생성할 공 프리팹

    private Transform   playerTransform;    // 플레이어의 위치
    private PoolManager ballPool;

    private void Awake()
    {
        StartCoroutine(CreateRandomPos());

        ballPool = new PoolManager(ballPrefab);
    }

    private IEnumerator CreateRandomPos()
    {
        while(true)
        {
            yield return new WaitForSeconds(waitSec);

            // Player의 위치 받아옴
            playerTransform   = PlayerController.instance.transform;
            // 플레이어의 position
            Vector3 targetPos = playerTransform.position;
            // 플레이어의 위치를 기점으로 randomPos 생성
            Vector3 randomPos = new Vector3(Random.Range(targetPos.x - radius, targetPos.x + radius),
                                            posY,
                                            Random.Range(targetPos.z - radius, targetPos.z + radius));
            
            // ball 생성
            //GameObject clone = Instantiate(ballPrefab, randomPos, Quaternion.identity, transform);
            ActiveBall(randomPos);

            yield return null;
        }
    }

    private void ActiveBall(Vector3 pos)
    {
        GameObject ball = ballPool.ActivatePoolItem();
        ball.transform.position = pos;
        ball.transform.parent   = transform;
        ball.transform.rotation = Quaternion.identity;
        ball.GetComponent<PoolingThis>().Setup(ballPool);
    }

}
