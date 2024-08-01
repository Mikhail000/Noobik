using System;
using System.Collections;
using UnityEngine;

public class PureAnimation : MonoBehaviour
{
    public TransformChanges LastChanges { get; private set; }

    private Coroutine _lastAnimation;
    private MonoBehaviour _contex;

    public PureAnimation(MonoBehaviour contex)
    {
        _contex = contex;
    }

    public void Play(float duration, Func<float, TransformChanges> body)
    {
        if (_lastAnimation != null)
            _contex.StopCoroutine(_lastAnimation);

        _lastAnimation = _contex.StartCoroutine(GetAnimation(duration, body));

    }

    private IEnumerator GetAnimation(float duration, Func<float, TransformChanges> body)
    {
        var expiredSeconds = 0f;
        var progress = 0f;

        while (progress < 1)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / duration;

            LastChanges = body.Invoke(progress);

            yield return null;
        }
    }
}
