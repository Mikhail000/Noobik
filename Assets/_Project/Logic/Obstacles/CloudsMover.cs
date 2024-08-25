using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class CloudsMover : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    public float speed = 2f; // �������� �������� ���������
    public float distance = 20f; // ���������� ����� ������� (-20 �� 20)

    private void FixedUpdate()
    {
        float xPosition = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        Vector3 newPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        rigidbody.MovePosition(newPosition);
    }
}
