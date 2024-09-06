using System.Collections;
using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f; // Сила отталкивания
    [SerializeField] private float bounceDamping = 0.5f; // Затухание отталкивания (0 = нет затухания, 1 = полное затухание)

    private Rigidbody rigidbody;

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.gameObject.tag == "Player")
        {
    
            // Получаем Rigidbody объекта игрока
            Rigidbody rb = other.GetComponent<Rigidbody>();
    
            if (rb != null)
            {
                // Получаем направление скорости объекта (направление его движения)
                Vector3 incomingDirection = rb.velocity.normalized;
    
                // Получаем нормаль к поверхности пружинящей платформы
                Vector3 surfaceNormal = transform.up; // Используем вектор вверх платформы в качестве нормали
    
                // Рассчитываем вектор отражения от поверхности
                Vector3 reflectDirection = Vector3.Reflect(incomingDirection, surfaceNormal);
    
                rb.velocity = Vector3.Reflect(incomingDirection, surfaceNormal) * bounceForce;
    
                // Применяем силу отталкивания, уменьшая ее в зависимости от затухания
                //rb.velocity = reflectDirection * bounceForce * (1f - bounceDamping);
    
    
                // Выводим отладочное сообщение
                Debug.Log("Player Bounced Off the Spring Surface!");
            }
        }
    
    }

}
