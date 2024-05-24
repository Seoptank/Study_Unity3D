using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [Header("Input Key")]
    [SerializeField]
    private KeyCode     runKey = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode     jumpKey = KeyCode.Space;

    private RotateToMouse               rotateToMouses;
    private MovementCharacterController movement;
    private Status                      status;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);

        // 마우스 보이지 않게, 현위치 고정
        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouses  = GetComponent<RotateToMouse>();
        movement        = GetComponent<MovementCharacterController>();
        status          = GetComponent<Status>();
    }

    private void Update()
    {
        UpdateRotation();
        UpdateMove();
        UpdateJump();
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중 이라면
        if( x != 0 || z != 0 )
        {
            bool isRun = false;
            
            // 전방 이동중일 경우만 뛰기 가능(뒤로 뛰기 불가)
            if( z > 0 ) isRun = Input.GetKey(runKey);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
        }

        movement.MoveTo(new Vector3(x,0,z));
    }
    private void UpdateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouses.UpdateToRotate(mouseX, mouseY);
    }
    private void UpdateJump()
    {
        if (Input.GetKeyDown(jumpKey))
            movement.JumpTo();
    }
}
