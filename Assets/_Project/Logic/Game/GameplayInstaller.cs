using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mobileCanvasForInput;
    [SerializeField] private GameObject camera;
    
    public override void InstallBindings()
    {
        BindMessageBroker();
        BindInput();
        BindPlayer();
        BindCamera();
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
            //Container.InstantiatePrefab(mobileCanvasForInput);
            // ...
        }
    }
    
    private void BindPlayer()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(playerPrefab);
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
    }

    private void BindCamera()
    {
        CameraFollow cinema = Container.InstantiatePrefabForComponent<CameraFollow>(camera);
        Container.Bind<CameraFollow>().FromInstance(cinema).AsSingle().NonLazy();    
    }
}
