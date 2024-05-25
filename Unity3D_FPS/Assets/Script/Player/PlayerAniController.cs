using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniController : MonoBehaviour
{
    private Animator ani;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        set => ani.SetFloat("MovementSpeed", value);
        get => ani.GetFloat("MovementSpeed");
    }

    public void OnReload()
    {
        ani.SetTrigger("OnReload");
    }

    public void Play(string stateName,int layer,float normalizedTime)
    {
        ani.Play(stateName, layer, normalizedTime);
    }

    public bool CurAniIs(string name)
    {
        return ani.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
