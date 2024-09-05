using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySur : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bounceForce = 5f; // Сила отскока
    [SerializeField] private float _friction = 0.8f; // Коэффициент трения для уменьшения отскока

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, с чем столкнулись
        if (collision.gameObject.tag == "Bouncy")
        {
            // Берем первую точку контакта
            ContactPoint contact = collision.contacts[0];

            // Вычисляем нормаль поверхности в точке контакта
            Vector3 surfaceNormal = contact.normal;

            // Рассчитываем силу отскока в противоположном направлении
            Vector3 bounceDirection = Vector3.Reflect(_rigidbody.velocity, surfaceNormal);

            // Уменьшаем скорость отскока на коэффициент трения
            bounceDirection *= _friction;

            // Применяем силу отскока
            _rigidbody.velocity = bounceDirection * _bounceForce;
        }
    }
}
