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
    private TextMeshProUGUI     textWeaponName;     // ���� �̸� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI     textAmmo;           // ź UI
    [SerializeField]
    private Image               imageWeaponIcon;    // ���� ������
    [SerializeField]
    private Sprite[]            spriteWeaponIcon;   // ���� ������ ��������Ʈ
    [SerializeField]
    private GameObject          magazineUIPrefab;   // źâ ������
    [SerializeField]
    private Transform           magazineParent;     // źâ�� ��ġ�Ǵ� �θ� ��ġ
    
    private List<GameObject>    magazineList;       // źâ UI ����Ʈ

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
        // weapon�� ��ϵ� ����ŭ źâ ������ ����/��Ȱ��ȭ
        magazineList = new List<GameObject>();

        for (int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        // ���� źâ ������ŭ�� Ȱ��ȭ
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
        // ���� ����ִ� źâ �� ��ŭ�� ȭ��� ������Ʈ
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
