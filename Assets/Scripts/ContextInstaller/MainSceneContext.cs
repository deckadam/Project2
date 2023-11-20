using UI;
using UnityEngine;
using Zenject;

namespace ContextInstaller.Main
{
    public class MainSceneContext : MonoInstaller
    {
        [SerializeField] private MainUI _mainUI;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainUI).AsSingle();
        }
    }
}