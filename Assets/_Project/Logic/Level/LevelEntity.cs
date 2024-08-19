using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelEntity : MonoBehaviour
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public Transform FinishPoint { get; private set; }
    [field: SerializeField] public Finish Finish { get; private set; }
    [field: SerializeField] public Transform CameraPoint { get; private set; }
    [field: SerializeField] public List<Checkpoint> Checkpoints { get; private set; }

    [field: SerializeField] public bool isPassed { get; private set; }

    private int _nextCheckpointIndex = default;
    private Transform _lastPassedCheckpoint;
    private Vector3 _offsetSpawn = new Vector3(0f, 0.2f, 0f);

    private Player _player;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IMessagePublisher _publisher;
    private IDisposable _stopDisposable;
    private PreloaderLevelService _preloaderService;

    [Inject]
    private void Construct(Player player, IMessageReceiver receiver, IMessagePublisher publisher, 
        PreloaderLevelService preloaderService)
    {
        _player = player;
        _receiver = receiver;
        _disposable = new();

        _preloaderService = preloaderService;   
    }

    private void Start()
    {
        SetCheckpoints();

        _lastPassedCheckpoint = StartPoint;

        _receiver.Receive<DieMessage>().Subscribe(ResetLevel).AddTo(_disposable);
        _receiver.Receive<ReachedFinal>().Subscribe(LoadNextLevel).AddTo(_disposable);

    }

    public void PassedThroughCheckpoint(Checkpoint checkpoint)
    {
        if (Checkpoints.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            _nextCheckpointIndex++;
            _lastPassedCheckpoint = checkpoint.GetTranform();
        }
        else
        {

        }
    }

    public void PassedThoughFinish()
    {
        // когда дошли до финиша
        // ревард экран
        //_publisher.Publish(new FinishedLevel());
        // сохраняем уровень, как пройденный и запускаем следующий...
        _preloaderService.SaveAndLoadNextLevel();
    }

    private void SetCheckpoints()
    {
        Checkpoints.ForEach(checkpoint => checkpoint.SetTrackOfCheckpoints(this));
        Finish.SetFinishForLevel(this);
    }


    private void ResetLevel(DieMessage dieMessage)
    {
        Debug.Log("DIE");
        _player.SetPosition(_lastPassedCheckpoint.position);
        _stopDisposable?.Dispose();
    }

    private void LoadNextLevel(ReachedFinal recheadMessage)
    {

    }

}
