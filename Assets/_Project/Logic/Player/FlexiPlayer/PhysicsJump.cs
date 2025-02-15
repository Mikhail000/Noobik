using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PhysicsJump : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private JumpFX _fx;
    [SerializeField] private float _length;
    [SerializeField] private float _duration;
    [SerializeField] private float _jumpForce;

    private PureAnimation _playTime;
    private bool _isJumping;

    private void Awake()
    {
        _playTime = new PureAnimation(this);
    }

    public void Jump(Vector3 direction)
    {

        if (_rigidbody == null)
        {
            Debug.LogWarning("Rigidbody is missing or destroyed!");
            return;
        }

        // �������� ������� ����������� ��� ����� �������
        Vector3 projectedDirection = _surfaceSlider.Project(direction.normalized);

        Vector3 startVelocity = _rigidbody.velocity;
        Vector3 targetVelocity = projectedDirection * _length / _duration;
        targetVelocity.y = _jumpForce;

        PureAnimation fxPlaytime = _fx.PlayAnimations(transform, _duration);

        _playTime.Play(_duration, (progress) =>
        {

            Vector3 currentVelocity = Vector3.Lerp(startVelocity, targetVelocity, progress);
            _rigidbody.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z)
            + fxPlaytime.LastChanges.Position;

            return null;

        });

    }
}
