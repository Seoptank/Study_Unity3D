using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField]
    private float   moveSpeed;                  // �����̴� �ӵ�
    private Vector3 moveVec = Vector3.zero;     // �����̴� ����
    private void Update()
    {
        transform.Translate(moveVec * moveSpeed * Time.deltaTime);
    }

    // �ܺο��� x���� z���� �޾� ����
    public void MoveTo(float x,float z)
    {
        moveVec = new Vector3(x, 0, z);
    }
}
