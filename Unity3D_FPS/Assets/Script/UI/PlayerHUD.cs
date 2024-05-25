using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rifle weapon;        

    [Header("Weapon")]
    [SerializeField]
    private TextMeshProUGUI     textWeaponName;     // 무기 이름 텍스트
    [SerializeField]
    private TextMeshProUGUI     textAmmo;           // 탄 UI
    [SerializeField]
    private Image               imageWeaponIcon;    // 무기 아이콘
    [SerializeField]
    private Sprite[]            spriteWeaponIcon;   // 무기 아이콘 스프라이트
    [SerializeField]
    private GameObject          magazineUIPrefab;   // 탄창 프리팹
    [SerializeField]
    private Transform           magazineParent;     // 탄창이 배치되는 부모 위치
    
    private List<GameObject>    magazineList;       // 탄창 UI 리스트

    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();

        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
        weapon.onMagazineEvent.AddListener(UpdateMagazineHUD);
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcon[(int)weapon.WeaponName];
    }
    private void SetupMagazine()
    {
        // weapon에 등록된 수만큼 탄창 프리팹 생성/비활성화
        magazineList = new List<GameObject>();

        for (int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        // 현재 탄창 개수만큼만 활성화
        for (int i = 0; i < weapon.CurMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }


    private void UpdateAmmoHUD(int maxAmmo,int curAmmo)
    {
        textAmmo.text = $"<size=+20>{maxAmmo}/</size>{curAmmo}";
    }
    private void UpdateMagazineHUD(int curMagazine)
    {
        // 현재 들고있는 탄창 수 만큼만 화면상에 업데이트
        for (int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < curMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }
}
