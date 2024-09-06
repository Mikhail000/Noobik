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

    public void Jump(Vector3 direction)
    {

        if (_rigidbody == null)
        {
            Debug.LogWarning("Rigidbody is missing or destroyed!");
            return;
        }

        Vector3 startVelocity = _rigidbody.velocity;

        Vector3 target = startVelocity + (direction * _length);

        //Vector3 targetVelocity = direction * _length / _duration;


        //targetVelocity.y = _jumpForce;  // Устанавливаем силу прыжка по оси Y

        PureAnimation fxPlaytime = _fx.PlayAnimations(transform, _duration);

        _playTime.Play(_duration, (progress) =>
        {

            _rigidbody.velocity = Vector3.Lerp(startVelocity, target, progress) + fxPlaytime.LastChanges.Position;
            // _rigidbody.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z)
            // + fxPlaytime.LastChanges.Position;



            return null;
        });

    }
}
