using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMoveComponent : MonoBehaviour
{
    [Header("Components")] [Space(5)] [SerializeField]
    private Rigidbody rigidbody;

    [Space(5)] [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform rearTransform;
    [Space(5)] [SerializeField] private Transform tiltPivot;
    [Space(5)] [SerializeField] private Transform centerMass;

    [Header("Parameters")] [Space(5)] [SerializeField]
    private bool directionAlignment;

    [SerializeField] private float moveSpeed; //10f
    [SerializeField] private float jumpForce; //5f;
    [SerializeField] private float turnSpeed; //500
    [SerializeField] private float tiltAngle; //30f; // максимальный угол наклона
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float wheelRotationSpeed;
    

    private bool _isGrounded;
    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;

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
        //rigidbody.centerOfMass = centerMass.position;
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.5f);

        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);

        if (_currentMoveDirection != Vector3.zero)
        {
            Move();
            Turn();
            Tilt();
            RotateWheels();

            //UpdateWheels();
        }
        else
        {
            ResetTilt();
            //ResetYAxisRotation();
        }
    }

    private void GetDirectionEvent(MoveMessage moveMessage)
    {
        _targetMoveDirection = new Vector3(moveMessage.Delta.x, moveMessage.Delta.y, moveMessage.Delta.z);

        _stopDisposable?.Dispose();
    }

    private void GetJumpEvent(JumpMessage jumpMessage)
    {
        if (_isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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

    private void Move()
    {
        Vector3 forwardMove = _currentMoveDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + forwardMove);
    }

    private void Turn()
    {
        float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));
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

    private void RotateWheels()
    {
        float rotationAmount = moveSpeed * wheelRotationSpeed * Time.fixedDeltaTime;
        frontTransform.Rotate(Vector3.right, rotationAmount);
        rearTransform.Rotate(Vector3.right, rotationAmount);
    }
    
    private void ResetYAxisRotation()
    {
        Vector3 currentEulerAngles = rigidbody.rotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(currentEulerAngles.x, 0f, currentEulerAngles.z);
        rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, targetRotation, tiltSpeed * Time.fixedDeltaTime));
    }
}