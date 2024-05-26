using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem  particle;
    private PoolManager     poolManager;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Setup(PoolManager newPool)
    {
        poolManager = newPool;
    }

    private void Update()
    {
        if (!particle.isPlaying)
            poolManager.DeactivatePoolItems(this.gameObject);
    }
}
