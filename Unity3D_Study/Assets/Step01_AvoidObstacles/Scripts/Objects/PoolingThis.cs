using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingThis : MonoBehaviour
{
    private PoolManager pool;

    public void Setup(PoolManager newPool)
    {
        pool = newPool;

        StartCoroutine(DeactivateThis());
    }

    private IEnumerator DeactivateThis()
    {
        yield return new WaitForSeconds(5);
        pool.DeactivatePoolItem(this.gameObject);
    }
}
