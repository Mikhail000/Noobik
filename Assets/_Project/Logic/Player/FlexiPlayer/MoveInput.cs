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
    [SerializeField] private WheelGroundChecker groundDetection;
    [SerializeField] private BicycleAnimator _bikeAnimator;
    [SerializeField] private Transform tiltPivot;

    [Header("Movement Parameters")]
    [Space(5)]

    [SerializeField] private float onGroundSpeed;
    [SerializeField] private float onAirSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float tiltAngle;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private PhysicsJump physicsJump;

    private float speed;
    private Vector3 _offset;
    private bool _isGrounded;
    private Vector3 _currentMoveDirection;
    private Vector3 _targetMoveDirection;

    private bool _isJumping;
    private Vector3 _airVelocity;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IDisposable _stopDisposable;
    private IDisposable _lockRotationSubscription;

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

        rigidbody.centerOfMass = new Vector3(0f, .1f, -.3f);

    }

    private void FixedUpdate()
    {

        _currentMoveDirection =
            Vector3.Lerp(_currentMoveDirection, _targetMoveDirection, Time.fixedDeltaTime * tiltSpeed);


        AdjustBalanceOnAir();

        Turn();

        if (groundDetection.IsGrounded)
        {
            if (_currentMoveDirection != Vector3.zero)
            {
                Tilt();
                Move(_currentMoveDirection);
                _bikeAnimator.RotateWheels(_currentMoveDirection.magnitude);

            }
            else
            {
                ResetTilt();
            }

        }
        else
        {
            MaintainAirVelocity();
        }
    }


    private void MaintainAirVelocity()
    {
        // Продолжать движение с сохраненной скоростью в воздухе
        if (_airVelocity != Vector3.zero)
        {
            Move(_airVelocity.normalized);
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
        //Quaternion targetTiltRotation = Quaternion.Euler(tiltPivot.localRotation.eulerAngles.x,
        //    tiltPivot.localRotation.eulerAngles.y, 0f);
        //tiltPivot.localRotation =
        //    Quaternion.Slerp(tiltPivot.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);

        Quaternion targetTiltRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,
            transform.localRotation.eulerAngles.y, 0f);
        transform.localRotation =
            Quaternion.Slerp(transform.localRotation, targetTiltRotation, tiltSpeed * Time.fixedDeltaTime);
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
        if (groundDetection.IsGrounded)
        {
            Vector3 vec = new Vector3(0, 1, 0); // Направление прыжка
            physicsJump.Jump(vec);
            _isJumping = true;

            _airVelocity = _currentMoveDirection.normalized * speed;
        }
    }

    private void AdjustBalanceOnAir()
    {
        Debug.Log($"IsGrounded: {groundDetection.IsGrounded}");
        if (groundDetection.IsGrounded)
        {
            Debug.Log("Grounded: UnlockRotationX");
            UnlockRotationX();
            speed = onGroundSpeed;

            // Отменяем запланированную блокировку вращения, если велосипед снова на земле
            _lockRotationSubscription?.Dispose();
            _lockRotationSubscription = null;
        }
        else
        {
            Debug.Log("Not Grounded: Preparing to LockRotationX"); // Отладка
            Debug.Log($"Current _lockRotationSubscription: {_lockRotationSubscription}"); // Проверка состояния подписки

            // Проверяем, если уже есть активная подписка, не создаем новую
            if (_lockRotationSubscription == null)
            {
                Debug.Log("Creating new subscription for LockRotationX..."); // Добавить отладку

                _lockRotationSubscription = Observable.Timer(TimeSpan.FromSeconds(0.75))
                    .TakeUntil(Observable.EveryUpdate().Where(_ =>
                    {
                        Debug.Log("Checking if grounded to cancel timer..."); // Добавить отладку
                        return groundDetection.IsGrounded;
                    })) // Отмена, если велосипед снова на земле
                    .Subscribe(_ =>
                    {
                        LockRotationX();
                        speed = onAirSpeed;
                        Debug.Log("GET LOCK ROTATION");
                        _lockRotationSubscription = null;  // Сбрасываем подписку после выполнения
                    },
                    () => Debug.Log("Subscription completed or cancelled.")) // Добавить отладку для завершения подписки
                    .AddTo(_disposable); // Добавляем в CompositeDisposable для управления подписками
            }
            else
            {
                Debug.Log("Subscription already exists, not creating a new one.");
            }
        }

    }

    private void LockRotationX()
    {
        Debug.Log("GET LOCK ROTATION");
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