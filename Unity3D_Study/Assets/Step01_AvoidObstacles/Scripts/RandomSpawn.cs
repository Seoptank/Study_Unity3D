using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float minX, maxX, minZ, maxZ;
    [SerializeField]
    private float posY;
    [SerializeField]
    private float waitSec;
    [SerializeField]
    private GameObject  ballPrefab;
    [SerializeField]
    private Transform   parent;


    private void Awake()
    {
        StartCoroutine(CreateRandomPos());
    }

    private IEnumerator CreateRandomPos()
    {
        while(true)
        {
            yield return new WaitForSeconds(waitSec);
            
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), posY, Random.Range(minZ, maxZ));

            GameObject clone = Instantiate(ballPrefab, randomPos, Quaternion.identity, parent);

            yield return null;
        }
    }

}
