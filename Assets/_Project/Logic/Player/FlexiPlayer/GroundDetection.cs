using Unity.VisualScripting;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        // ���������, ������������� �� ��������� � ������ (���� "Ground")
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ���� ��������� ��������� ������������� � ������
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = false;
        }
    }
}
