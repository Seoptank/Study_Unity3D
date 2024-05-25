using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Speed Variables")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
}
