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
    [SerializeField]
    private KeyCode     reloadKey = KeyCode.R;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip   walkClip;
    [SerializeField]
    private AudioClip   runClip;

    private RotateToMouse               rotateToMouses;
    private MovementCharacterController movement;
    private PlayerAniController         anim;
    private Status                      status;
    private AudioSource                 audio;
    private Rifle                       rifle;
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
        anim            = GetComponent<PlayerAniController>();
        audio           = GetComponent<AudioSource>();
        
        rifle           = GetComponentInChildren<Rifle>();
    }

    private void Update()
    {
        UpdateRotation();
        UpdateMove();
        UpdateJump();

        UpdateWeaponAction();
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중 이라면
        if (x != 0 || z != 0)
        {
            bool isRun = false;

            // 전방 이동중일 경우만 뛰기 가능(뒤로 뛰기 불가)
            if (z > 0) isRun = Input.GetKey(runKey);

            // 실질적인 스피드 제어
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;

            // 애니메이션 제어
            anim.MoveSpeed = isRun == true ? 1 : 0.5f;

            // 사운드 제어
            audio.clip = isRun == true ? runClip : walkClip;

            // isPlaying 여부에 따라 재생중에는 재생하지 않도록
            if (!audio.isPlaying)
            {
                audio.loop = true;
                audio.Play();
            }
        }
        // 멈춘 경우
        else
        {
            movement.MoveSpeed = 0;
            anim.MoveSpeed     = 0;

            if(audio.isPlaying)
            {
                audio.Stop();
            }
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
        {
            movement.JumpTo();
        }
    }
    private void UpdateWeaponAction()
    {
        if(Input.GetMouseButtonDown(0))
            rifle.StartWeaponAction();
        else if(Input.GetMouseButtonUp(0))
            rifle.StopWeaponAction();

        if (Input.GetKeyDown(reloadKey))
            rifle.StartReload();
    }

    public void TakeDmg(int dmg)
    {
        bool isDie = status.DecreaseHP(dmg);

        if(isDie)
        {
            Debug.Log("Die");
        }
    }
}
