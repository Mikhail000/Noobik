using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimation : MonoBehaviour
{
    [SerializeField] private float _riseHeight = 2f; 
    [SerializeField] private float _riseDuration = 0.2f; 
    [SerializeField] private float _pauseDurationTop = 1f; 
    [SerializeField] private float _lowerDuration = 0.5f; 
    [SerializeField] private float _pauseDurationBottom = 0.5f; 

    [SerializeField] private AnimationCurve _riseCurve; // Кривая для подъема
    [SerializeField] private AnimationCurve _lowerCurve; // Кривая для опускания

    private Vector3 _initialPosition;
    private float _elapsedTime;
    private bool _isRising;
    private bool _isPausingTop;
    private bool _isLowering;
    private bool _isPausingBottom;

    private void Start()
    {
        _initialPosition = transform.position;
        _elapsedTime = 0f;
        _isRising = true;
    }

    private void Update()
    {
        if (_isRising)
        {
            RiseSpike();
        }
        else if (_isPausingTop)
        {
            PauseAtTop();
        }
        else if (_isLowering)
        {
            LowerSpike();
        }
        else if (_isPausingBottom)
        {
            PauseAtBottom();
        }
    }

    private void RiseSpike()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / _riseDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isRising = false;
            _isPausingTop = true;
            _elapsedTime = 0f;
        }
        float curveValue = _riseCurve.Evaluate(progress);
        transform.position = Vector3.Lerp(_initialPosition, _initialPosition + Vector3.up * _riseHeight, curveValue);
    }

    private void PauseAtTop()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _pauseDurationTop)
        {
            _isPausingTop = false;
            _isLowering = true;
            _elapsedTime = 0f;
        }
    }

    private void LowerSpike()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / _lowerDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isLowering = false;
            _isPausingBottom = true;
            _elapsedTime = 0f;
        }
        float curveValue = _lowerCurve.Evaluate(progress);
        transform.position = Vector3.Lerp(_initialPosition + Vector3.up * _riseHeight, _initialPosition, curveValue);
    }

    private void PauseAtBottom()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _pauseDurationBottom)
        {
            _isPausingBottom = false;
            _isRising = true; // Цикл начинается снова
            _elapsedTime = 0f;
        }
    }
}
