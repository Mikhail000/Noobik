using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class MoveComponent : MonoBehaviour
{
    #region FIELDS
    
    [Header("Components")] [Space(5)] 
    [SerializeField] private Rigidbody rigidbody;
    
    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private WheelCollider rearWheel;
    [Space(5)]
    [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform rearTransform;
    [Space(5)]
    [SerializeField] private Transform tiltPivot;

    [Header("Parameters")] [Space(5)] [SerializeField]
    private bool directionAlignment;

    [SerializeField] private float moveSpeed; //10f
    [SerializeField] private float turnSpeed; //500
    [SerializeField] private float tiltAngle; //30f; // максимальный угол наклона
    [SerializeField] private float tiltSpeed; //5f;
    [SerializeField] private float jumpForce; //5f;
    [SerializeField] private float brakeTorque;
    
    private bool _isGrounded;
    private float _rayOffset = .5f;
    private float _rayLength = .7f;

    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IDisposable _stopDisposable;

    #endregion
    
    [Inject]
    private void Construct(IMessageReceiver receiver)
    {
        _receiver = receiver;
        _disposable = new();
    }

    private void Start()
    {
        _receiver.Receive<MoveMessage>().Subscribe(GetDirectionEvent).AddTo(_disposable);
        _receiver.Receive<JumpMessage>().Subscribe(GetJumpEvent).AddTo(_disposable);
        _receiver.Receive<StopMessage>().Subscribe(GetStopEvent).AddTo(_disposable);
        
        rigidbody.centerOfMass = new Vector3(0, 0.5f, 0.0f);
        Debug.Log(rigidbody.centerOfMass);
    }

    private void OnDestroy()
    {
        _disposable.Dispose();

        _stopDisposable?.Dispose();
    }

    private void FixedUpdate()
    {
        Debug.Log(frontWheel.brakeTorque);
        
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * _rayOffset, Vector3.down, _rayLength,
            LayerMask.GetMask("Default"));
        Debug.DrawRay(transform.position + Vector3.up * _rayOffset, Vector3.down * _rayLength, Color.blue);
        
        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);
        
        
        if (_currentMoveDirection != Vector3.zero)
        {

            Move();
            Turn();
            
            //Tilt();
            UpdateWheels();
        }
        else
        {
            ApplyBrakes();
            ResetTilt();
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
        
        /*_targetMoveDirection = Vector3.zero;
        _stopDisposable?.Dispose();
        _stopDisposable = Observable.EveryUpdate()
            .TakeWhile(_ => rearWheel.motorTorque != 0)
            .Subscribe(_ =>
            {
                rearWheel.motorTorque = Mathf.Lerp(rearWheel.motorTorque, 0, Time.fixedDeltaTime * brakeTorque);
            }); */
        
        _targetMoveDirection = Vector3.zero;

        // Мгновенная остановка
        rearWheel.motorTorque = 0f;
        frontWheel.brakeTorque = brakeTorque;
        rearWheel.brakeTorque = brakeTorque;

        // Также можно обнулить скорость Rigidbody для мгновенной остановки
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        
    }
    
    
    private void Move()
    {
        
        //rearWheel.motorTorque = _currentMoveDirection.z * moveSpeed;
        frontWheel.motorTorque = moveSpeed;
        //rearWheel.motorTorque = moveSpeed;
        frontWheel.brakeTorque = 0f;
        rearWheel.brakeTorque = 0f;
        
    }

    private void ApplyBrakes()
    {
        frontWheel.motorTorque = 0f;
        rearWheel.motorTorque = 0f;
        
        frontWheel.brakeTorque = brakeTorque;
        rearWheel.brakeTorque = brakeTorque;
    }
    
    private void Turn()
    {
        //float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        //Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, targetAngle, transform.rotation.z);
        //rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));
        
        float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);
        // Только изменяем угол поворота по оси Y
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, newRotation, Time.fixedDeltaTime * turnSpeed));
    }

    private void Tilt()
    {
        if (_currentMoveDirection != Vector3.zero)
        {
            float targetTiltAngle = -_currentMoveDirection.z * tiltAngle;
            Quaternion targetTiltRotation =
                Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetTiltAngle);
            
            tiltPivot.rotation =
                Quaternion.Slerp(tiltPivot.rotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
            
        }
        else
        {
            ResetTilt();
        }
    }
    
    private void ResetTilt()
    {
        Quaternion targetTiltRotation = Quaternion.Euler(0f, 0f, 0f);
        tiltPivot.localRotation = Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
    }
    
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontWheel, frontTransform);
        UpdateSingleWheel(rearWheel, rearTransform);
    }
    
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }
    
}