using UnityEngine;

public class SlimePhysicJump : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private SlimeJumpFx _fx;
    [SerializeField] private float _length;
    [SerializeField] private float _duration;
    [SerializeField] private float _jumpForce;

    private PureAnimation _playTime;
    private bool _isJumping;

    private void Awake()
    {
        _playTime = new PureAnimation(this);
    }

    public void Bounc(Collider surface,Vector3 direction)
    {
        Vector3 jumpDirection = CalculateJumpDirection(surface);
        Jump(jumpDirection);
    }


    private Vector3 CalculateJumpDirection(Collider other)
    {

        // �������� ������� ����������� � ����� ��������
        Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        Vector3 surfaceNormal = (_rigidbody.worldCenterOfMass - closestPoint).normalized;

        // ������������ ������� ����������� �������� ����������
        Vector3 forwardDirection = _rigidbody.velocity.normalized;

        // ���������� ����������� �� 45 �������� �� ��������� � ������� ����������� � �������� ������
        Vector3 jumpDirection = (surfaceNormal + forwardDirection).normalized;

        // ��������, ��� ��� jumpDirection ��������� � �������� ��������� ���� (�� ������� ������������ ��� ��������������)
        jumpDirection = Vector3.RotateTowards(surfaceNormal, jumpDirection, Mathf.Deg2Rad * 45f, 0);

        return jumpDirection;

        // �������� ������� �������� ������
        //Vector3 currentVelocity = _rigidbody.velocity;
        // ���������� ����� ���� ������ � ��������� ����� �� �����������
        //Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        // ������������ ������� ����������� � ����� ��������
        //Vector3 surfaceNormal = (_rigidbody.worldCenterOfMass - closestPoint).normalized;
        // ������������ ����������� ��������� ������� �������� ������������ ������� �����������
        //Vector3 reflectDirection = Vector3.Reflect(currentVelocity.normalized, surfaceNormal);
        // ����������� ������ ����������� ������
        //reflectDirection.Normalize();
        //return reflectDirection;


        // ���������� ����� ���� ������ � ��������� ����� �� �����������
        //Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        // ������������ ������� �� ����� �������� �� ������ ����
        //Vector3 jumpDirection = (_rigidbody.worldCenterOfMass - closestPoint).normalized;
        //return jumpDirection;
    }

    public void Jump(Vector3 direction)
    {
        if (_rigidbody == null)
        {
            Debug.LogWarning("Rigidbody is missing or destroyed!");
            return;
        }

        // �������� ������� �������� ��� ����������� ����������� ������
        _rigidbody.velocity = Vector3.zero;

        // ��������� ���� ������ �� ����������� � ��������� ������ FX
        _rigidbody.AddForce(direction * _jumpForce, ForceMode.Impulse);

        // ����������� �������� �������
        if (_fx != null)
        {
            _fx.PlayAnimations(transform, _duration); // ������: ����������������� ������� 0.5 ���
        }
    }
}
