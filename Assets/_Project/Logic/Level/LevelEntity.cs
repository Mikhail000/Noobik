using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using YG;
using Zenject;

public class LevelEntity : MonoBehaviour
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public Transform FinishPoint { get; private set; }
    [field: SerializeField] public Finish Finish { get; private set; }
    [field: SerializeField] public Transform CameraPoint { get; private set; }
    [field: SerializeField] public List<Checkpoint> Checkpoints { get; private set; }

    [field: SerializeField] public bool isPassed { get; private set; }
    [field: SerializeField] public Material skyMaterial { get; private set; }

    private int _nextCheckpointIndex = default;
    private Transform _lastPassedCheckpoint;
    private Vector3 _offsetSpawn = new Vector3(0f, 0.2f, 0f);

    private Player _player;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private IMessagePublisher _publisher;
    private PreloaderLevelService _preloaderService;

    [Inject]
    private void Construct(Player player, IMessageReceiver receiver, IMessagePublisher publisher, 
        PreloaderLevelService preloaderService)
    {
        _player = player;
        _receiver = receiver;
        _publisher = publisher; 
        _disposable = new();

        _preloaderService = preloaderService;   
    }

    private void Start()
    {
        RenderSettings.skybox = skyMaterial;

        SetCheckpoints();

        _lastPassedCheckpoint = StartPoint;

        _receiver.Receive<DieMessage>().Subscribe(StopOnDie).AddTo(_disposable);
        _receiver.Receive<LaunchNextLevelEvent>().Subscribe(LoadNextLevel).AddTo(_disposable);
        _receiver.Receive<RestartEvent>().Subscribe(RestartLevel).AddTo(_disposable);

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
        _publisher.Publish(new OnShowNextLevelWindow());

    }

    private void SetCheckpoints()
    {
        Checkpoints.ForEach(checkpoint => checkpoint.SetTrackOfCheckpoints(this));
        Finish.SetFinishForLevel(this);
    }


    private void StopOnDie(DieMessage dieMessage)
    {
        _publisher.Publish(new OnShowRestartWindow());
    }

    private void RestartLevel(RestartEvent restartEvent)
    {
        YG2.InterstitialAdvShow();
        _player.SetPosition(_lastPassedCheckpoint.position);
    }

    private void LoadNextLevel(LaunchNextLevelEvent launchNextLevelEvent)
    {
        YG2.InterstitialAdvShow();
        _preloaderService.SaveAndLoadNextLevel();
    }

}
