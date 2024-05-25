using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5; // 카메라 x축 회전속도
    [SerializeField]                    
    private float rotCamYAxisSpeed = 5; // 카메라 y축 회전속도

    [SerializeField]
    private float limitMinX = -80;      // x축 카메라 회전 범위(최소)
    [SerializeField]
    private float limitMaxX =  50;      // x축 카메라 회전 범위(최대)
    
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateToRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        // x축 회전(상/히) 움직임의 경우 움직이는 범위를 설정
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
