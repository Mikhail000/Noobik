using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimberRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200; // �������� �������� � �������� � �������

    void FixedUpdate()
    {

        float rotationY = rotationSpeed * Time.deltaTime;

        transform.Rotate(0, rotationY, 0);
    }
}
