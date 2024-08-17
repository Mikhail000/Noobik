using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class Finish : MonoBehaviour
{
    private LevelEntity _level;

    public void SetFinishForLevel(LevelEntity level)
    {
        _level = level;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player p))
        {
            _level.PassedThoughFinish();
            Debug.Log("REACHED FINISH");
        }
    }
}


