using System.Collections;
using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f; // ���� ������������
    [SerializeField] private float bounceDamping = 0.5f; // ��������� ������������ (0 = ��� ���������, 1 = ������ ���������)

    private Rigidbody rigidbody;

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.gameObject.tag == "Player")
        {
    
            // �������� Rigidbody ������� ������
            Rigidbody rb = other.GetComponent<Rigidbody>();
    
            if (rb != null)
            {
                // �������� ����������� �������� ������� (����������� ��� ��������)
                Vector3 incomingDirection = rb.velocity.normalized;
    
                // �������� ������� � ����������� ���������� ���������
                Vector3 surfaceNormal = transform.up; // ���������� ������ ����� ��������� � �������� �������
    
                // ������������ ������ ��������� �� �����������
                Vector3 reflectDirection = Vector3.Reflect(incomingDirection, surfaceNormal);
    
                rb.velocity = Vector3.Reflect(incomingDirection, surfaceNormal) * bounceForce;
    
                // ��������� ���� ������������, �������� �� � ����������� �� ���������
                //rb.velocity = reflectDirection * bounceForce * (1f - bounceDamping);
    
    
                // ������� ���������� ���������
                Debug.Log("Player Bounced Off the Spring Surface!");
            }
        }
    
    }

}
