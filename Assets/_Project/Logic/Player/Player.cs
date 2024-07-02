using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _playerTransform;

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }
}
