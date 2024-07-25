using System;
using UniRx;
using UnityEngine;
using Zenject;

public class MoveInput : MonoBehaviour
{
    [Header("Components")] [Space(5)] [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField] private SurfaceSlider surfaceSlider;
    [SerializeField] private Transform tiltPivot;

    [Header("Parameters")] [Space(5)] [SerializeField]
    private float speed;

    [SerializeField] private float turnSpeed; //500
    [SerializeField] private float tiltAngle; //30f; // максимальный угол наклона
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float jumpForce;


    private Vector3 _offset;
    private bool _isGrounded;
    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;

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
        //Debug.Log($"IsGrounded: {_isGrounded}");


        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);

        if (_currentMoveDirection != Vector3.zero && _isGrounded)
        {
            Turn();
            Tilt();
            Move(_currentMoveDirection);
        }
        else
        {
            //ResetTilt();
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = surfaceSlider.Project(direction);
        _offset = directionAlongSurface * (speed * Time.fixedDeltaTime);

        rigidbody.MovePosition(rigidbody.position + _offset);
    }

    private void Turn()
    {
        float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);
        rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * speed));
        
        //float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);
        //// Только изменяем угол поворота по оси Y
        //Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        //rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, newRotation, Time.fixedDeltaTime * turnSpeed));
    }

    private void Tilt()
    {
        float targetTiltAngle = -_currentMoveDirection.x * tiltAngle;
        Quaternion targetTiltRotation = Quaternion.Euler(tiltPivot.localRotation.eulerAngles.x,
            tiltPivot.localRotation.eulerAngles.y, targetTiltAngle);
        tiltPivot.localRotation =
            Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
    }

    private void ResetTilt()
    {
        Quaternion targetTiltRotation = Quaternion.Euler(tiltPivot.localRotation.eulerAngles.x,
            tiltPivot.localRotation.eulerAngles.y, 0f);
        tiltPivot.localRotation =
            Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
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
        if (_isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}