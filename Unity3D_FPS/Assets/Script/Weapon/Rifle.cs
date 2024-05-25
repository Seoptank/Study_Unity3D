using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public class Rifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting   weaponSetting;  // ���� ����
    private float           lastAttackTime; // ������ �߻� �ð�

    [Header("Fire Effect")]
    [SerializeField]
    private GameObject      fireEffect;     // �ѱ� Effect

    [Header("Casing Spawn")]
    [SerializeField]
    private Transform       casingSpawnPos; // ź�� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip       takeoutClip;    // ���� ���� ����
    [SerializeField]
    private AudioClip       fireClip;       // ���� �߻� ����
    [SerializeField]
    private AudioClip       reloadClip;     // ������ ����

    // ������
    private bool            isReload = false;

    private AudioSource         audioSource;
    private PlayerAniController anim;
    private CasingPool          casingPool;

    public WeaponName   WeaponName => weaponSetting.name;
    public int          CurMagazine => weaponSetting.curMagazine;
    public int          MaxMagazine => weaponSetting.maxMagazine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        casingPool  = GetComponent<CasingPool>();
        anim        = GetComponentInParent<PlayerAniController>();

        // ź��/ źâ �� �ִ�� ����
        weaponSetting.curAmmo       = weaponSetting.maxAmmo;
        weaponSetting.curMagazine   = weaponSetting.maxMagazine;
    }

    private void OnEnable()
    {
        // Ȱ��ȭ�� ���� ���� ���� ����
        PlaySound(takeoutClip);

        fireEffect.SetActive(false);
        
        // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ ź ��/źâ �� ������ �����Ѵ�.
        onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);
        onMagazineEvent.Invoke(weaponSetting.curAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        // ��Ŭ��
        if(type == 0 )
        {
            if (isReload) return;

            // ���� ����
            if (weaponSetting.isAutomaticAttack)
                StartCoroutine("OnAttackLoop");
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        // ��Ŭ��( ���� ����)
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        if (isReload || weaponSetting.curMagazine <= 0 ) return;

        if (weaponSetting.curAmmo == weaponSetting.maxAmmo) return;

        // ���� �׼� ���� ������ �õ��ϸ� �׼� ���� �� ������
        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while(true)
        {
            OnAttack();

            yield return null;
        }
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        // ������ �ִ�,���� ���
        anim.OnReload();
        PlaySound(reloadClip);

        while(true)
        {
            // �������� ����� ���
            if(!audioSource.isPlaying && anim.CurAniIs("Movement"))
            {
                isReload = false;

                // źâ �� �ֽ�ȭ
                weaponSetting.curMagazine--;
                onMagazineEvent.Invoke(weaponSetting.curMagazine);

                // ź ���� �ִ�� ���� UI�ֽ�ȭ 
                weaponSetting.curAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);

                yield break;
            }
            yield return null;
        }
    }

    private void OnAttack()
    {
        // ������ ���ݽð��� �߻��� ���� �� �ð��� �߻� ���� �ð��� �Ѿ��
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // �ٴ� ���� ��� ���� �Ұ� 
            if (anim.MoveSpeed > 0.5f) return;

            if (weaponSetting.curAmmo <= 0) return;

            weaponSetting.curAmmo--;

            onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);

            // ������ ���� �ð� ����
            lastAttackTime = Time.time;

            // ���� �ִ� ���
            anim.Play("Fire", -1, 0);

            // �ѱ� Effect ���
            StartCoroutine("OnFireEffect");

            // ���� ���� ���
            PlaySound(fireClip);

            // Casing ����
            casingPool.SpawnCaing(casingSpawnPos, transform.right);
        }
    }

    private IEnumerator OnFireEffect()
    {
        fireEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        fireEffect.SetActive(false);
    }


    private void PlaySound(AudioClip newClip)
    {
        audioSource.Stop();             // ���� ���� ����
        audioSource.clip = newClip;     // ���ο� Ŭ�� ����
        audioSource.Play();             // ���ο� ���� ���
    }
}
