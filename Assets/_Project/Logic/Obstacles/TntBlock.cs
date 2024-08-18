using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntBlock : MonoBehaviour
{
    [SerializeField] private float explosionForce = 20f; // ���� ������
    [SerializeField] private float explosionRadius = 5f;  // ������ ������
    [SerializeField] private float upwardModifier = 1f;   // ����������� ������� ��� ������
    [SerializeField] private ParticleSystem explosionEffect;  // ������ ������ (��������, �������)
    private void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ������������ ��������� � �������� � ����� "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���� ���� ������ ������, ������� ��� � ����� �����
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }

            // �������� Rigidbody ����������
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // ��������� ���� ������ � Rigidbody ����������
                playerRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }

            // ���������� �����
            Destroy(gameObject);
        }
    }

}
