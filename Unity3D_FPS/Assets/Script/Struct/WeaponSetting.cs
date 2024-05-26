public enum WeaponName
{
    Rifle = 0,
}

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName name;             // ���� �̸�
    public int      weaponDmg;          // ���� ����
    public int      curAmmo;            // ���� ź��
    public int      maxAmmo;            // ���� ź��
    public int      curMagazine;        // ���� źâ �� 
    public int      maxMagazine;        //
    public float    attackRate;         // ���� �ӵ�
    public float    attackDistance;     // ���� �ִ� �Ÿ�
    public bool     isAutomaticAttack;  // ���� ���� ����
}
