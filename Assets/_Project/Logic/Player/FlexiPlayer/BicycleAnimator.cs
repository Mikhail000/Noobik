using UnityEngine;

public class BicycleAnimator : MonoBehaviour
{
    [SerializeField] private Transform[] wheels; // Массив колес, которые будут вращаться
    [SerializeField] private float rotationSpeedMultiplier = -1.0f; // Множитель скорости вращения

    public void RotateWheels(float bikeSpeed)
    {
        // Вращаем каждое колесо по оси X
        foreach (Transform wheel in wheels)
        {
            float rotationAmount = bikeSpeed * rotationSpeedMultiplier * Time.fixedDeltaTime;
            wheel.Rotate(rotationAmount, 0f, 0f);
        }
    }
}
