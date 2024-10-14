using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class MoveComponent : MonoBehaviour
{
    #region FIELDS

    [Header("Components")]
    [Space(5)]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private WheelGroundChecker groundDetecter;
    [SerializeField] private PhysicsJump physicsJump;
    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private WheelCollider rearWheel;
    [Space(5)]

    [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform rearTransform;
    [Space(5)]

    [SerializeField] private Transform tiltPivot;

    [Header("Parameters")]
    [Space(5)]
    [SerializeField]
    private bool directionAlignment;

    [SerializeField] private float acceleration;
    [SerializeField] private float breakingForce;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float tiltAngle;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float steerSpeed;

    [Header("Anti-Roll Bar Parameters")]
    [SerializeField] private float antiRollBarStiffness = 50f; // Жесткость анти-ролл бара
    [SerializeField] private float wheelBaseWidth = 1.2f; // Расстояние между колесами


    private bool _isGrounded;
    private Vector3 _localDown;
    private Vector3 _rayOrigin;
    private float _rayOffset = .5f;
    private float _rayLength = .7f;

    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;
    private bool _isMoving;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;


    #endregion

    [Inject]
    private void Construct(IMessageReceiver receiver)
    {
        _receiver = receiver;
        _disposable = new();
    }

    private void Start()
    {
        _disposable = new CompositeDisposable();

        _receiver.Receive<MoveMessage>().Subscribe(GetDirectionEvent).AddTo(_disposable);
        _receiver.Receive<JumpMessage>().Subscribe(GetJumpEvent).AddTo(_disposable);
        _receiver.Receive<StopMessage>().Subscribe(GetStopEvent).AddTo(_disposable);
        _receiver.Receive<DieMessage>().Subscribe(GetDieEvent).AddTo(_disposable);

        rigidbody.centerOfMass = new Vector3(0f, 0.35f, -0.3f);
        //rigidbody.centerOfMass = new Vector3(0f, 0.1f, -0.2f);
    }

    private void OnDestroy() =>
        _disposable.Dispose();


    private void FixedUpdate()
    {

        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);

        //ApplyAntiRollBar();

        AdjustBalanceOnAir();


        if (_currentMoveDirection != Vector3.zero && _isMoving)
        {

            Turn();
            Tilt();
            Move();
            //Steering(); 
            UpdateWheels();
        }
        else
        {
            ResetTilt();
        }

    }

    private void ApplyAntiRollBar()
    {
        WheelHit frontHit;
        WheelHit rearHit;

        float travelFront = 1.0f;
        float travelRear = 1.0f;

        // Проверяем, касаются ли колеса земли
        bool groundedFront = frontWheel.GetGroundHit(out frontHit);
        bool groundedRear = rearWheel.GetGroundHit(out rearHit);

        if (groundedFront)
        {
            travelFront = (-frontWheel.transform.InverseTransformPoint(frontHit.point).y - frontWheel.radius) / frontWheel.suspensionDistance;
        }

        if (groundedRear)
        {
            travelRear = (-rearWheel.transform.InverseTransformPoint(rearHit.point).y - rearWheel.radius) / rearWheel.suspensionDistance;
        }

        // Вычисляем силу анти-ролл бара
        float antiRollForce = (travelFront - travelRear) * antiRollBarStiffness;

        if (groundedFront)
        {
            rigidbody.AddForceAtPosition(frontWheel.transform.up * -antiRollForce, frontWheel.transform.position);
        }

        if (groundedRear)
        {
            rigidbody.AddForceAtPosition(rearWheel.transform.up * antiRollForce, rearWheel.transform.position);
        }
    }


    private void GetDirectionEvent(MoveMessage moveMessage)
    {
        _targetMoveDirection = new Vector3(moveMessage.Delta.x, moveMessage.Delta.y, moveMessage.Delta.z);

        _isMoving = true;
    }

    private void GetJumpEvent(JumpMessage jumpMessage)
    {
        if (groundDetecter.IsGrounded)
            physicsJump.Jump(_currentMoveDirection);
    }

    private void GetStopEvent(StopMessage stopMessage)
    {

        _targetMoveDirection = Vector3.zero;
        ApplyBrakes();
        _isMoving = false;

    }

    private void GetDieEvent(DieMessage dieMessage)
    {
        ApplyBrakes();
    }

    private void Move()
    {
        if (groundDetecter.IsGrounded)
        {
            frontWheel.brakeTorque = 0f;
            rearWheel.brakeTorque = 0f;

            frontWheel.motorTorque = acceleration;
            rearWheel.motorTorque = acceleration;

        }
        else ApplyBrakes();

    }

    private void ApplyBrakes()
    {
        frontWheel.motorTorque = 0;
        rearWheel.motorTorque = 0;

        frontWheel.brakeTorque = breakingForce;
        rearWheel.brakeTorque = breakingForce;
    }

    private void Steering()
    {
        float steerAngle = Mathf.Clamp(_currentMoveDirection.x * steerSpeed, -45f, 45f);
        frontWheel.steerAngle = steerAngle;
        //rearWheel.steerAngle = steerAngle;
        //frontTransform.localRotation = Quaternion.Euler(0, steerAngle, 0);
    }

    private void Turn()
    {

        float targetAngle = Mathf.Atan2(_currentMoveDirection.x, _currentMoveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, newRotation, Time.fixedDeltaTime * turnSpeed));
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
        Quaternion targetTiltRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,
            transform.localRotation.eulerAngles.y, 0f);
        transform.localRotation =
            Quaternion.Slerp(transform.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
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

    private void AdjustBalanceOnAir()
    {
        if (groundDetecter.IsGrounded)
        {
            UnlockRotationX();
        }
        else
        {
            LockRotationX();
            Turn();
        }

    }

    private void LockRotationX()
    {

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void UnlockRotationX()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnDrawGizmos()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Установите цвет для Gizmos
        Gizmos.color = Color.red;

        // Нарисуйте сферу в центре массы Rigidbody
        Gizmos.DrawSphere(rigidbody.worldCenterOfMass, 0.1f);

        // Нарисуйте линию от позиции объекта до центра массы
        Gizmos.DrawLine(transform.position, rigidbody.worldCenterOfMass);
    }

}