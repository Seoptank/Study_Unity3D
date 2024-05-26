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
    private WeaponSetting   weaponSetting;  // 무기 설정
    private float           lastAttackTime; // 마지막 발사 시간

    [Header("Fire Effect")]
    [SerializeField]
    private GameObject      fireEffect;     // 총구 Effect

    [Header("Spawn Pos")]
    [SerializeField]
    private Transform       casingSpawnPos; // 탄피 생성 위치
    [SerializeField]
    private Transform       bulletSpawnPos; // 총알 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip       takeoutClip;    // 무기 장착 사운드
    [SerializeField]
    private AudioClip       fireClip;       // 무기 발사 사운드
    [SerializeField]
    private AudioClip       reloadClip;     // 재장전 사운드

    // 재장전
    private bool            isReload = false;

    private AudioSource         audioSource;
    private PlayerAniController anim;
    private CasingPool          casingPool;
    private ImpactPool          impactPool;
    private Camera              mainCam;

    public WeaponName   WeaponName => weaponSetting.name;
    public int          CurMagazine => weaponSetting.curMagazine;
    public int          MaxMagazine => weaponSetting.maxMagazine;

    private void Awake()
    {
        mainCam = Camera.main;

        audioSource = GetComponent<AudioSource>();
        casingPool  = GetComponent<CasingPool>();
        impactPool  = GetComponent<ImpactPool>();
        anim        = GetComponentInParent<PlayerAniController>();

        // 탄수/ 탄창 수 최대로 설정
        weaponSetting.curAmmo       = weaponSetting.maxAmmo;
        weaponSetting.curMagazine   = weaponSetting.maxMagazine;
    }

    private void OnEnable()
    {
        // 활성화시 무기 장착 사운드 진행
        PlaySound(takeoutClip);

        fireEffect.SetActive(false);
        
        // 무기가 활성화 될 때 해당 무기의 탄 수/탄창 수 정보를 갱신한다.
        onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);
        onMagazineEvent.Invoke(weaponSetting.curAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        // 좌클릭
        if(type == 0 )
        {
            if (isReload) return;

            // 연속 공격
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
        // 좌클릭( 공격 종료)
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        if (isReload || weaponSetting.curMagazine <= 0 ) return;

        if (weaponSetting.curAmmo == weaponSetting.maxAmmo) return;

        // 무기 액션 도중 재장전 시도하면 액션 종료 후 재장전
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

        // 재장전 애니,사운드 재생
        anim.OnReload();
        PlaySound(reloadClip);

        while(true)
        {
            // 재장전이 종료된 경우
            if(!audioSource.isPlaying && anim.CurAniIs("Movement"))
            {
                isReload = false;

                // 탄창 수 최신화
                weaponSetting.curMagazine--;
                onMagazineEvent.Invoke(weaponSetting.curMagazine);

                // 탄 수를 최대로 설정 UI최신화 
                weaponSetting.curAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);

                yield break;
            }
            yield return null;
        }
    }

    private void OnAttack()
    {
        // 마지막 공격시간이 발생한 이후 그 시간이 발사 간격 시간을 넘어가면
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // 뛰는 중일 경우 공격 불가 
            if (anim.MoveSpeed > 0.5f) return;

            if (weaponSetting.curAmmo <= 0) return;

            weaponSetting.curAmmo--;

            onAmmoEvent.Invoke(weaponSetting.maxAmmo, weaponSetting.curAmmo);

            // 마지막 공격 시간 저장
            lastAttackTime = Time.time;

            // 공격 애니 재생
            anim.Play("Fire", -1, 0);

            // 총구 Effect 재생
            StartCoroutine("OnFireEffect");

            // 공격 사운드 재생
            PlaySound(fireClip);

            // Casing 진행
            casingPool.SpawnCaing(casingSpawnPos, transform.right);

            Shoot();
        }
    }

    private void Shoot()
    {
        Ray         ray;
        RaycastHit  hit;
        Vector3     targetPos = Vector3.zero;

        // 화면 중앙 좌표
        ray = mainCam.ViewportPointToRay(Vector2.one * 0.5f);

        // 공격 사거리 내 부딪히는 물체가 있음
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
            targetPos = hit.point;
        // 공격 사거리 내 부딪히는 물체가 없음
        else
            targetPos = ray.origin + ray.direction * weaponSetting.attackDistance;
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // 첫번째 Raycast연산으로 얻어진 targetPos를 목표로 설정하고 총구 시작점부터 Raycast연산
        Vector3 atkDir = (targetPos - bulletSpawnPos.position).normalized;
        if (Physics.Raycast(bulletSpawnPos.position, atkDir, out hit, weaponSetting.attackDistance))
        {
            impactPool.SpawnImpact(hit);

            if(hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemyAI>().TakeDmg(weaponSetting.weaponDmg);
            }
        }
        Debug.DrawRay(bulletSpawnPos.position, atkDir * weaponSetting.attackDistance, Color.blue);
    }
    private IEnumerator OnFireEffect()
    {
        fireEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        fireEffect.SetActive(false);
    }


    private void PlaySound(AudioClip newClip)
    {
        audioSource.Stop();             // 기존 사운드 중지
        audioSource.clip = newClip;     // 새로운 클립 삽입
        audioSource.Play();             // 새로운 사운드 재생
    }
}
