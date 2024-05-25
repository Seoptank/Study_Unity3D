using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingPool : MonoBehaviour
{
    [SerializeField]
    private GameObject  casingPrefab;
    private PoolManager casingPool;

    private void Awake()
    {
        casingPool = new PoolManager(casingPrefab);
    }

    public void SpawnCaing(Transform pos,Vector3 dir)
    {
        GameObject csing = casingPool.ActivatePoolItem();
        csing.transform.position = pos.position;
        csing.transform.parent   = pos;
        csing.transform.rotation = Random.rotation;
        csing.GetComponent<Casing>().Setup(casingPool, dir);
    }
}
