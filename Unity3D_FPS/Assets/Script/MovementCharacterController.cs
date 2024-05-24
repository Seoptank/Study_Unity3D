using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour
{
    [SerializeField]
    private float   moveSpeed;  // �̵� �ӵ�
    [SerializeField]
    private float   jumpForce;  // ���� ��
    [SerializeField]
    private float   gravity;    // �߷� ���

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
        // �߷� ����
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
        // ���� �ִ� ��츸 ���� �����ϰ�
        if (characterController.isGrounded)
            moveVec.y = jumpForce;
    }
}
