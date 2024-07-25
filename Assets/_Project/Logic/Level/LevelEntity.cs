using System;
using System.Collections.Generic;
using UniRx;
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

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IDisposable _stopDisposable;

    [Inject]
    private void Construct(Player player, IMessageReceiver receiver)
    {
        _player = player;
        _receiver = receiver;
        _disposable = new();
    }
    
    private void Start()
    {
        SetCheckpoints();

        _lastCheckpoint = StartPoint;  


        //Debug.Log(_receiver);
        //Debug.Log(_disposable);
        _receiver.Receive<DieMessage>().Subscribe(ResetLevel).AddTo(_disposable);

    }

    public void PassedThroughCheckpoint(Checkpoint checkpoint)
    {
        if (Checkpoints.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            _nextCheckpointIndex++;
            _lastCheckpoint = checkpoint.GetTranform();
        }
        else
        {
            
        }
    }

    private void SetCheckpoints() => Checkpoints.ForEach(checkpoint => checkpoint.SetTrackOfCheckpoints(this));
    

    private void ResetLevel(DieMessage dieMessage)
    {
        Debug.Log("DIE");
        _player.SetPosition(_lastCheckpoint.position);
        _stopDisposable?.Dispose();
    }

}
