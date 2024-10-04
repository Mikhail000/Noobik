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

        // Получаем нормаль поверхности в точке контакта
        Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        Vector3 surfaceNormal = (_rigidbody.worldCenterOfMass - closestPoint).normalized;

        // Рассчитываем текущее направление движения велосипеда
        Vector3 forwardDirection = _rigidbody.velocity.normalized;

        // Определяем направление на 45 градусов по отношению к нормали поверхности и движению вперед
        Vector3 jumpDirection = (surfaceNormal + forwardDirection).normalized;

        // Убедимся, что наш jumpDirection находится в пределах разумного угла (не слишком вертикальный или горизонтальный)
        jumpDirection = Vector3.RotateTowards(surfaceNormal, jumpDirection, Mathf.Deg2Rad * 45f, 0);

        return jumpDirection;

        // Получаем текущую скорость игрока
        //Vector3 currentVelocity = _rigidbody.velocity;
        // Определяем центр масс игрока и ближайшую точку на поверхности
        //Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        // Рассчитываем нормаль поверхности в точке контакта
        //Vector3 surfaceNormal = (_rigidbody.worldCenterOfMass - closestPoint).normalized;
        // Рассчитываем направление отражения текущей скорости относительно нормали поверхности
        //Vector3 reflectDirection = Vector3.Reflect(currentVelocity.normalized, surfaceNormal);
        // Нормализуем вектор направления прыжка
        //reflectDirection.Normalize();
        //return reflectDirection;


        // Определяем центр масс игрока и ближайшую точку на поверхности
        //Vector3 closestPoint = other.ClosestPoint(_rigidbody.worldCenterOfMass);
        // Рассчитываем нормаль от точки контакта до центра масс
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

        // Обнуляем текущую скорость для обеспечения постоянного прыжка
        _rigidbody.velocity = Vector3.zero;

        // Применяем силу прыжка по направлению и добавляем эффект FX
        _rigidbody.AddForce(direction * _jumpForce, ForceMode.Impulse);

        // Проигрываем анимацию эффекта
        if (_fx != null)
        {
            _fx.PlayAnimations(transform, _duration); // Пример: продолжительность эффекта 0.5 сек
        }
    }
}
