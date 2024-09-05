using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySur : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bounceForce = 5f; // ���� �������
    [SerializeField] private float _friction = 0.8f; // ����������� ������ ��� ���������� �������

    private void OnCollisionEnter(Collision collision)
    {
        // ���������, � ��� �����������
        if (collision.gameObject.tag == "Bouncy")
        {
            // ����� ������ ����� ��������
            ContactPoint contact = collision.contacts[0];

            // ��������� ������� ����������� � ����� ��������
            Vector3 surfaceNormal = contact.normal;

            // ������������ ���� ������� � ��������������� �����������
            Vector3 bounceDirection = Vector3.Reflect(_rigidbody.velocity, surfaceNormal);

            // ��������� �������� ������� �� ����������� ������
            bounceDirection *= _friction;

            // ��������� ���� �������
            _rigidbody.velocity = bounceDirection * _bounceForce;
        }
    }
}
