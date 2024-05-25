using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float       deactivateTime = 5.0f;    // ���� �� ��Ȱ��ȭ ������ �ð�
    [SerializeField]
    private float       casingSpin = 1.0f;        // ź�� ȸ�� ���
    [SerializeField]
    private AudioClip[] casingClips;              // ź�� ���� Ŭ�� �迭

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
        // ź�� ���� ���� ���
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
