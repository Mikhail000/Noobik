using UniRx;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mobileCanvasForInput;
    
    public override void InstallBindings()
    {
        BindMessageBroker();
        BindInput();
        BindPlayer();
    }


    private void BindMessageBroker()
    {
        MessageBroker broker = new();

        Container.Bind<IMessagePublisher>().FromInstance(broker).AsSingle();
        Container.Bind<IMessageReceiver>().FromInstance(broker).AsSingle();
    }

    private void BindInput()
    {
        
        Container.Bind<InputLayout>().AsSingle().NonLazy();
        Container.Bind<InputReader>().AsSingle().NonLazy();
        
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Container.InstantiatePrefab(mobileCanvasForInput);
        }

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            // ...
        }
    }
    
    private void BindPlayer()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(playerPrefab);
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
    }
}
