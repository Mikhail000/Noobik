using UnityEngine;

public class PistonAnimation : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float extendDistance;

    [SerializeField] private float pushDuration;
    [SerializeField] private float pullDuration;

    [SerializeField] private float extendPause;
    [SerializeField] private float reducePause;

    [SerializeField] private AnimationCurve _pullCurve;
    [SerializeField] private AnimationCurve _pushCurve;

    private Vector3 _initialPosition;
    private float _elapsedTime;
    private bool _isPushing;
    private bool _isPulling;
    private bool _isPausingPushed;
    private bool _isPausingPulled;

    private void Start()
    {
        _initialPosition = rb.position;
        _elapsedTime = 0f;
        _isPushing = true;
    }

    private void Update()
    {
        if (_isPushing)
        {
            Push();
        }
        else if (_isPausingPushed)
        {
            PauseAtPush();
        }
        else if (_isPulling)
        {
            Pull();
        }
        else if (_isPausingPulled)
        {
            PauseAtPull();
        }
    }

    private void Push()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / pushDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isPushing = false;
            _isPausingPushed = true;
            _elapsedTime = 0f;
        }
        float curveValue = _pushCurve.Evaluate(progress);
        transform.localPosition = Vector3.Lerp(_initialPosition, _initialPosition + Vector3.up * extendDistance, curveValue);
    }

    private void Pull()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / pullDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isPulling = false;
            _isPausingPulled = true;
            _elapsedTime = 0f;
        }
        float curveValue = _pullCurve.Evaluate(progress);
        transform.localPosition = Vector3.Lerp(_initialPosition + Vector3.up * extendDistance, _initialPosition, curveValue);
    }

    private void PauseAtPush()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= extendPause)
        {
            _isPausingPushed = false;
            _isPulling = true;
            _elapsedTime = 0f;
        }
    }

    private void PauseAtPull()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= reducePause)
        {
            _isPausingPushed = false;
            _isPushing = true; // Цикл начинается снова
            _elapsedTime = 0f;
        }
    }

}
