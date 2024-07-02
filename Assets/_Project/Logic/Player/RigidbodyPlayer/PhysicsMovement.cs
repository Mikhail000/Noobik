using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    private Vector3 _offset;

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surfaceSlider.Project(direction);
        _offset = directionAlongSurface * (_speed * Time.fixedDeltaTime);
        
        _rigidbody.MovePosition(_rigidbody.position + _offset);
    }
    
}
