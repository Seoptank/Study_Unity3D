using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour
{
    [SerializeField]
    private float   moveSpeed;  // 이동 속도
    [SerializeField]
    private float   jumpForce;  // 점프 힘
    [SerializeField]
    private float   gravity;    // 중력 계수

    private Vector3 moveVec;

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 중력 적용
        if(!characterController.isGrounded)
        {
            moveVec.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveVec * Time.deltaTime);
    }

    public void MoveTo(Vector3 dir)
    {
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        moveVec = new Vector3(dir.x * moveSpeed, moveVec.y, dir.z * moveSpeed);
    }

    public void JumpTo()
    {
        // 땅에 있는 경우만 점프 가능하게
        if (characterController.isGrounded)
            moveVec.y = jumpForce;
    }
}
