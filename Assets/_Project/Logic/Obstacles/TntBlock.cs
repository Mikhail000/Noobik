using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntBlock : MonoBehaviour
{
    [SerializeField] private float explosionForce = 20f; // Сила взрыва
    [SerializeField] private float explosionRadius = 5f;  // Радиус взрыва
    [SerializeField] private float upwardModifier = 1f;   // Модификатор подъема при взрыве
    [SerializeField] private ParticleSystem explosionEffect;  // Эффект взрыва (например, частицы)
    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что столкновение произошло с объектом с тегом "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Если есть эффект взрыва, создаем его в месте бочки
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }

            // Получаем Rigidbody велосипеда
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Применяем силу взрыва к Rigidbody велосипеда
                playerRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }

            // Уничтожаем бочку
            Destroy(gameObject);
        }
    }

}
