using UnityEngine;
using Zenject;

namespace _Project.Logic.Menu
{
    public class MenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MenuWidgetContainer widgets;
        
        public override void InstallBindings()
        {
            Container.Bind<MenuWidgetContainer>().FromInstance(widgets);
        }
    }
}
