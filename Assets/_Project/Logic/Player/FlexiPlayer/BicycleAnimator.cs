using UnityEngine;

public class BicycleAnimator : MonoBehaviour
{
    [SerializeField] private Transform[] wheels; // ������ �����, ������� ����� ���������
    [SerializeField] private float rotationSpeedMultiplier = -1.0f; // ��������� �������� ��������

    public void RotateWheels(float bikeSpeed)
    {
        // ������� ������ ������ �� ��� X
        foreach (Transform wheel in wheels)
        {
            float rotationAmount = bikeSpeed * rotationSpeedMultiplier * Time.fixedDeltaTime;
            wheel.Rotate(rotationAmount, 0f, 0f);
        }
    }
}
