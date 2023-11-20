using Cinemachine;
using Player;
using UnityEngine;
using Zenject;

namespace ContextInstaller
{
    public class GameSceneContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CinemachineVirtualCamera _vCam;

        public override void InstallBindings()
        {
            Container.BindInstance(_playerData).AsSingle();
            Container.BindInstance(_vCam).AsSingle();
            Container.Bind<CameraController>().AsSingle();
        }
    }
}