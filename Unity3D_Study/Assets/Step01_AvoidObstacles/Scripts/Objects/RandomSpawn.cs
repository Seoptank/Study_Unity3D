using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float radius;   
    [SerializeField]
    private float posY;                     // ������ 
    [SerializeField]
    private float waitSec;                  // ��� �ð�
    [SerializeField]
    private GameObject  ballPrefab;         // ������ �� ������

    private Transform   playerTransform;    // �÷��̾��� ��ġ
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

            // Player�� ��ġ �޾ƿ�
            playerTransform   = PlayerController.instance.transform;
            // �÷��̾��� position
            Vector3 targetPos = playerTransform.position;
            // �÷��̾��� ��ġ�� �������� randomPos ����
            Vector3 randomPos = new Vector3(Random.Range(targetPos.x - radius, targetPos.x + radius),
                                            posY,
                                            Random.Range(targetPos.z - radius, targetPos.z + radius));
            
            // ball ����
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
