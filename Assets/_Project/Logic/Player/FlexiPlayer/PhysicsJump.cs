using UnityEngine;

public class PhysicsJump : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private JumpFX _fx;
    [SerializeField] private float _lengh;
    [SerializeField] private float _duration;

    private PureAnimation _playTime;

    private void Awake()
    {
        _playTime = new PureAnimation(this);
    }

    public void Jump(Vector3 direction)
    {
        Vector3 target = transform.position + (direction * _lengh);    
        Vector3 startPosition = transform.position;
        PureAnimation fxPlaytime = _fx.PlayAnimations(transform, _duration);

        _playTime.Play(_duration, (progress) =>
        {
            transform.position = Vector3.Lerp(startPosition, target, progress) + fxPlaytime.LastChanges.Position;
            //transform.localScale = fxPlaytime.LastChanges.Scale;
            return null;
        });
    }
}
