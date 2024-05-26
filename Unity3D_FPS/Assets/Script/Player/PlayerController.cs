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

        // ���콺 ������ �ʰ�, ����ġ ����
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

        // �̵��� �̶��
        if (x != 0 || z != 0)
        {
            bool isRun = false;

            // ���� �̵����� ��츸 �ٱ� ����(�ڷ� �ٱ� �Ұ�)
            if (z > 0) isRun = Input.GetKey(runKey);

            // �������� ���ǵ� ����
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;

            // �ִϸ��̼� ����
            anim.MoveSpeed = isRun == true ? 1 : 0.5f;

            // ���� ����
            audio.clip = isRun == true ? runClip : walkClip;

            // isPlaying ���ο� ���� ����߿��� ������� �ʵ���
            if (!audio.isPlaying)
            {
                audio.loop = true;
                audio.Play();
            }
        }
        // ���� ���
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
