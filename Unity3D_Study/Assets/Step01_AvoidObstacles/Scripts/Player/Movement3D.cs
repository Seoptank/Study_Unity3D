using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField]
    private float   moveSpeed;                  // 움직이는 속도
    private Vector3 moveVec = Vector3.zero;     // 움직이는 방향
    private void Update()
    {
        transform.Translate(moveVec * moveSpeed * Time.deltaTime);
    }

    // 외부에서 x값과 z값을 받아 적용
    public void MoveTo(float x,float z)
    {
        moveVec = new Vector3(x, 0, z);
    }
}
