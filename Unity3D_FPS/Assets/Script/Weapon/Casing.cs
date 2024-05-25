using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float       deactivateTime = 5.0f;    // 생성 후 비활성화 까지의 시간
    [SerializeField]
    private float       casingSpin = 1.0f;        // 탄피 회전 계수
    [SerializeField]
    private AudioClip[] casingClips;              // 탄피 사운드 클립 배열

    private Rigidbody   rigid;
    private AudioSource audio;
    private PoolManager pool;

    public void Setup(PoolManager newPool, Vector3 dir)
    {
        rigid   = GetComponent<Rigidbody>();
        audio   = GetComponent<AudioSource>();
        pool    = newPool;

        rigid.velocity = new Vector3(dir.x, 1.0f, dir.z);

        rigid.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                            Random.Range(-casingSpin, casingSpin),
                                            Random.Range(-casingSpin, casingSpin));

        StartCoroutine("DeactivateCasing");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탄피 사운드 랜덤 재생
        int random = Random.Range(0, casingClips.Length);

        audio.clip = casingClips[random];
        audio.Play();
    }

    private IEnumerator DeactivateCasing()
    {
        yield return new WaitForSeconds(deactivateTime);

        pool.DeactivatePoolItems(this.gameObject);
    }

}
