using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType { Normal, Blood,}
public class ImpactPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[]    impactPrefabs;  // ¿”∆Â∆Æ «¡∏Æ∆’ πËø≠
    private PoolManager[]   impactPool;     // ¿”∆Â∆Æ Pool πËø≠

    private void Awake()
    {
        impactPool = new PoolManager[impactPrefabs.Length];

        for (int i = 0; i < impactPrefabs.Length; ++i)
        {
            impactPool[i] = new PoolManager(impactPrefabs[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        if(hit.transform.CompareTag("Normal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if(hit.transform.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Blood, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public void OnSpawnImpact(ImpactType type,Vector3 point, Quaternion rot)
    {
        GameObject impact = impactPool[(int)type].ActivatePoolItem();
        impact.transform.position = point;
        impact.transform.rotation = rot;
        impact.GetComponent<Impact>().Setup(impactPool[(int)type]);
    }
}
