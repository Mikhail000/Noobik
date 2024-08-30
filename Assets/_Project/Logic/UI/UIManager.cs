using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject RestartWindow;
    [SerializeField] private GameObject NextLevelWindow;

    private IMessageReceiver _receiver;
    private CompositeDisposable _disposable;
    private DiContainer _container;

    [Inject]
    private void Construct(IMessageReceiver receiver, DiContainer container)
    {
        _receiver = receiver;
        _disposable = new CompositeDisposable();
        _container = container;
    }

    private void Start()
    {
        _receiver.Receive<OnShowRestartWindow>().Subscribe(ShowRestartWindow).AddTo(_disposable);
        _receiver.Receive<OnCloseRestartWindow>().Subscribe(CloseRestartWindow).AddTo(_disposable);
        _receiver.Receive<OnShowNextLevelWindow>().Subscribe(ShowNextLevelWindow).AddTo(_disposable);
        _receiver.Receive<OnCloseNextLevelWindow>().Subscribe(CloseNextLevelWindow).AddTo(_disposable);

        RestartWindow = _container.InstantiatePrefab(RestartWindow);
        NextLevelWindow = _container.InstantiatePrefab(NextLevelWindow);
    }

    private void ShowRestartWindow(OnShowRestartWindow onShowRestartWindow)
    {
        RestartWindow.SetActive(true);
    }

    private void CloseRestartWindow(OnCloseRestartWindow onCloseRestartWindow)
    {
        RestartWindow.SetActive(false);
    }

    private void ShowNextLevelWindow(OnShowNextLevelWindow onShowNextLevelWindow)
    {
        NextLevelWindow.SetActive(true);
    } 
    
    private void CloseNextLevelWindow(OnCloseNextLevelWindow onCloseNextLevelWindow)
    {
        NextLevelWindow.SetActive(false);
    }
}
