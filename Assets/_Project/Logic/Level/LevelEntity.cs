using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelEntity : MonoBehaviour
{
    [field:SerializeField] public Transform StartPoint { get; private set; }
    [field:SerializeField] public Transform FinishPoint { get; private set; }
    [field:SerializeField] public List<Checkpoint> Checkpoints { get; private set; }
    
    [field:SerializeField] public bool isPassed { get; private set; }

    private int _nextCheckpointIndex = default;
    private Transform _lastCheckpoint;

    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }
    
    private void Start()
    {
        foreach (var checkpoint in Checkpoints)
        {
            checkpoint.SetTrackOfCheckpoints(this);
        }
    }

    public void PassedThroughCheckpoint(Checkpoint checkpoint)
    {
        if (Checkpoints.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            _nextCheckpointIndex++;
            checkpoint.GetTranform();
        }
        else
        {
            
        }
    }

    private void SetCheckpoints() => Checkpoints.ForEach(checkpoint => checkpoint.SetTrackOfCheckpoints(this));
    
    

}
