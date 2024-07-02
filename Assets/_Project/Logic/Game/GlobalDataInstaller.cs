using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GlobalDataInstaller", menuName = "Installers/GlobalDataInstaller")]
public class GlobalDataInstaller : ScriptableObjectInstaller<GlobalDataInstaller>
{
    [field:SerializeField] public LevelStorage LevelStorage { get; private set; }
    public override void InstallBindings()
    {
        Container.Bind<LevelStorage>().FromInstance(LevelStorage).AsSingle();
    }
}