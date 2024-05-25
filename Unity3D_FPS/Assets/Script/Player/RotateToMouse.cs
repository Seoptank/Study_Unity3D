using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5; // ī�޶� x�� ȸ���ӵ�
    [SerializeField]                    
    private float rotCamYAxisSpeed = 5; // ī�޶� y�� ȸ���ӵ�

    [SerializeField]
    private float limitMinX = -80;      // x�� ī�޶� ȸ�� ����(�ּ�)
    [SerializeField]
    private float limitMaxX =  50;      // x�� ī�޶� ȸ�� ����(�ִ�)
    
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateToRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        // x�� ȸ��(��/��) �������� ��� �����̴� ������ ����
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle,float min,float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360)  angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
