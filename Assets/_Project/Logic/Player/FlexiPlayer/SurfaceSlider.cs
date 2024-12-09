using UnityEngine;
using UnityEngine.Rendering;

public class SurfaceSlider : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private float raycastDistance = 1.0f;

    private Vector3 _normal;

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    private void FixedUpdate()
    {
        // Запускаем луч вниз из заданной точки
        Ray ray = new Ray(raycastOrigin.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Обновляем нормаль поверхности, если луч что-то нашел
            _normal = hit.normal;
        }

        // Отрисовка луча для визуальной отладки
        Debug.DrawRay(raycastOrigin.position, Vector3.down * raycastDistance, Color.red);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _normal = collision.contacts[0].normal;
            Debug.Log(_normal);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward) * 0.6f);
    }
}
