using UnityEngine;
using YG;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private GameObject yandexGamePrefab;
    public override void InstallBindings()
    {
        BindLevelStorage();
        BindYandexGame();
        
    }

    private void BindLevelStorage()
    {
        Container.Bind<PreloaderLevelService>().AsSingle().NonLazy();
    }

    private void BindYandexGame()
    {
        YandexGame yandexGame = Container.InstantiatePrefabForComponent<YandexGame>(yandexGamePrefab);
        Container.Bind<YandexGame>().FromInstance(yandexGame).AsSingle().NonLazy();
    }
}
