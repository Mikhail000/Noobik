using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class JumpFX : MonoBehaviour
{
    [SerializeField] private AnimationCurve _yAnimation;
    [SerializeField] private float _height;
    [SerializeField] private PureAnimation _playTime;

    private void Awake()
    {
        _playTime = new PureAnimation(this);
    }

    public PureAnimation PlayAnimations(Transform jumper, float duration)
    {
        _playTime.Play(duration, (float progress)=>
            {
                Vector3 position = new Vector3(0, _height * _yAnimation.Evaluate(progress), 0);

                return new TransformChanges(position);

            });

        return _playTime;
    }
}
