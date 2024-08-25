using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGroundChecker : MonoBehaviour
{
    [SerializeField] private Transform frontWheel;
    [SerializeField] private Transform rearWheel;
    [SerializeField] private float rayLength = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private bool _isGrounded;
    private bool _frontHitted;
    private bool _rearHitted;

    public bool IsGrounded => _isGrounded;

    private void FixedUpdate()
    {
        _isGrounded = CheckForHit();
    }

    public bool CheckForHit()
    {
        _frontHitted = CheckGrounded(frontWheel);
        _rearHitted = CheckGrounded(rearWheel);
    
        return _frontHitted || _rearHitted;
    }

    // ����� ��� �������� �������������� ��� ������
    private bool CheckGrounded(Transform wheel)
    {
        // ���������� ������ ���� � ��� �����������
        Vector3 rayOrigin = wheel.position;
        Vector3 rayDirection = Vector3.down;

        // ��������� ��� ���� � ��������� ����������� � ������
        return Physics.Raycast(rayOrigin, rayDirection, rayLength, groundLayer);
    }

    // ��� ��������� ����� � ������ �����
    private void OnDrawGizmos()
    {
        Gizmos.color = _frontHitted ? Color.green : Color.red;
        Gizmos.DrawLine(frontWheel.position, frontWheel.position + Vector3.down * rayLength);

        Gizmos.color = _rearHitted ? Color.green : Color.red;
        Gizmos.DrawLine(rearWheel.position, rearWheel.position + Vector3.down * rayLength);
    }

}
