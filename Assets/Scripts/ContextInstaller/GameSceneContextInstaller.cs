using Cinemachine;
using Level;
using Player;
using UnityEngine;
using Zenject;

namespace ContextInstaller
{
    public class GameSceneContextInstaller : MonoInstaller
    {
        [SerializeField] private BlockPlacementManager _blockPlacementManager;
        [SerializeField] private BlockPlacementData _blockPlacementData;
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private MovingBlock block;

        public override void InstallBindings()
        {
            Container.BindFactory<MovingBlock, MovingBlock.Factory>().
                FromPoolableMemoryPool(x => x.WithInitialSize(20).
                    FromComponentInNewPrefab(block).
                    UnderTransformGroup("MovingBlocks"));
            
            Container.BindInstance(_blockPlacementManager).AsSingle();
            Container.BindInstance(_blockPlacementData).AsSingle();
            Container.BindInstance(_playerData).AsSingle();
            Container.BindInstance(_vCam).AsSingle();
            
            Container.Bind<CameraController>().AsSingle();
        }
    }
}