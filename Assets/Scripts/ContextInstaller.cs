using Player;
using UnityEngine;
using Zenject;

namespace ContextInstaller
{
    public class ContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerData _playerData;

        public override void InstallBindings()
        {
            Container.BindInstance(_playerData).AsSingle();
        }
    }
}