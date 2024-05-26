using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    [Header("Speed Variables")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    
    [Header("HP Variables")]
    [SerializeField]
    private int maxHP = 100;
    private int curHP;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public int CurHP=> curHP;
    public int MaxHP=> maxHP;

    private void Awake()
    {
        curHP = maxHP;
    }

    public bool DecreaseHP(int dmg)
    {
        int preHP = curHP;

        curHP = curHP - dmg > 0 ? curHP - dmg : 0;

        onHPEvent.Invoke(preHP, curHP);

        if(curHP == 0)
            return true;

        return false;
        
    }
}
