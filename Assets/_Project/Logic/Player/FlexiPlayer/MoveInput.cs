using System;
using UniRx;
using UnityEngine;
using Zenject;

public class MoveInput : MonoBehaviour
{
    [Header("Movement Components")]
    [Space(5)]

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private SurfaceSlider surfaceSlider;
    [SerializeField] private Transform tiltPivot;

    [Header("Movement Parameters")]
    [Space(5)]

    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed; //500
    [SerializeField] private float tiltAngle; //30f
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float jumpForce;
    //[SerializeField] private float jumpDuration = 1.0f;

    [Header("Jump Parameters")]
    [Space(5)]
    [SerializeField] private AnimationCurve jumpCurve; // Анимационная кривая для прыжка
    [SerializeField] private float jumpHeight = 2.0f; // Высота прыжка
    [SerializeField] private float jumpDuration = 1.0f;

    private Vector3 _offset;
    private bool _isGrounded;
    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;

    private bool _isJumping;
    private float _jumpStartTime;
    private Vector3 _jumpStartPosition;

    private float _rayOffset = .5f;
    private float _rayLength = .9f;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IDisposable _stopDisposable;

    [Inject]
    private void Construct(IMessageReceiver receiver)
    {
        _receiver = receiver;
        _disposable = new();
    }

    private void Start()
    {
        Debug.Log(_receiver);
        Debug.Log(_disposable);

        _receiver.Receive<MoveMessage>().Subscribe(GetDirectionEvent).AddTo(_disposable);
        _receiver.Receive<JumpMessage>().Subscribe(GetJumpEvent).AddTo(_disposable);
        _receiver.Receive<StopMessage>().Subscribe(GetStopEvent).AddTo(_disposable);

        rigidbody.centerOfMass = new Vector3(0, 0.5f, 0);
    }

    private void FixedUpdate()
    {

        _isGrounded = Physics.Raycast(transform.position + Vector3.up * _rayOffset, Vector3.down, _rayLength,
            LayerMask.GetMask("Default"));
        Debug.DrawRay(transform.position + Vector3.up * _rayOffset, Vector3.down * _rayLength, Color.blue);


        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);

        Turn();
        Tilt();

        if (_isJumping)
        {
            PerformJump();
        }
        else
        {
            _currentMoveDirection = Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);

            if (_currentMoveDirection != Vector3.zero && _isGrounded)
            {
                Move(_currentMoveDirection);
            }
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = surfaceSlider.Project(direction);
        _offset = directionAlongSurface * (speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(rigidbody.position + _offset);
        Debug.Log("MOOOOVIN");
    }

    private void Turn()
    {

        float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);
        rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));

    }

    private void Tilt()
    {
        if (Mathf.Abs(_targetMoveDirection.x) > 0 && Mathf.Abs(_targetMoveDirection.z) == 0)
        {
            return;
        }

        float targetTiltAngle;

        if (_targetMoveDirection.z < 0)
        {
            targetTiltAngle = _currentMoveDirection.x * tiltAngle;
        }
        else
        {
            targetTiltAngle = -_currentMoveDirection.x * tiltAngle;
        }

        Quaternion targetTiltRotation = Quaternion.Euler(tiltPivot.localRotation.eulerAngles.x,
            tiltPivot.localRotation.eulerAngles.y, targetTiltAngle);

        tiltPivot.localRotation = Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
    }

    private void ResetTilt()
    {
        Quaternion targetTiltRotation = Quaternion.Euler(tiltPivot.localRotation.eulerAngles.x,
            tiltPivot.localRotation.eulerAngles.y, 0f);
        tiltPivot.localRotation =
            Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
    }

    private void PerformJump()
    {
        float elapsedTime = Time.time - _jumpStartTime;
        if (elapsedTime < jumpDuration)
        {
            // Применение горизонтальной силы для поддержания движения
            Vector3 horizontalMovement = _currentMoveDirection * speed * Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + horizontalMovement);
        }
        else
        {
            _isJumping = false;
        }
    }

    private void GetDirectionEvent(MoveMessage moveMessage)
    {
        _targetMoveDirection = new Vector3(moveMessage.Delta.x, moveMessage.Delta.y, moveMessage.Delta.z);
        _stopDisposable?.Dispose();
    }

    private void GetStopEvent(StopMessage stopMessage)
    {
        _stopDisposable?.Dispose();

        _stopDisposable = Observable.EveryUpdate()
            .TakeWhile(_ => _currentMoveDirection != Vector3.zero)
            .Subscribe(_ =>
            {
                _targetMoveDirection = Vector3.zero;

                if (_currentMoveDirection.magnitude < 0.01f)
                {
                    _currentMoveDirection = Vector3.zero;
                    _stopDisposable?.Dispose();
                }
            });
    }

    private void GetJumpEvent(JumpMessage jumpMessage)
    {
        if (_isGrounded && !_isJumping)
        {
            _isJumping = true;
            _jumpStartTime = Time.time;
            rigidbody.AddForce(Vector3.up * Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight), ForceMode.VelocityChange);
        }
    }
}