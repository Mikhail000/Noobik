using UnityEngine;
using UnityEngine.Rendering;

public class SurfaceSlider : MonoBehaviour
{
    private Vector3 _normal;

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
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
