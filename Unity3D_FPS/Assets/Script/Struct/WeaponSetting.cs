public enum WeaponName
{
    Rifle = 0,
}

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName name;             // 무기 이름
    public int      weaponDmg;          // 무기 피해
    public int      curAmmo;            // 현재 탄수
    public int      maxAmmo;            // 최재 탄수
    public int      curMagazine;        // 현재 탄창 수 
    public int      maxMagazine;        //
    public float    attackRate;         // 공격 속도
    public float    attackDistance;     // 공격 최대 거리
    public bool     isAutomaticAttack;  // 연속 공격 여부
}
