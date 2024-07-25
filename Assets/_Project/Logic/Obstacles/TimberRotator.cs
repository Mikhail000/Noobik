using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimberRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200; // Скорость вращения в градусах в секунду

    void FixedUpdate()
    {

        float rotationY = rotationSpeed * Time.deltaTime;

        transform.Rotate(0, rotationY, 0);
    }
}
