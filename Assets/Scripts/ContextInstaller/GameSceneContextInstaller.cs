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
        [SerializeField] private MovingBlock _movingBlock;
        [SerializeField] private LevelFinishBlock _levelFinishBlock;

        public override void InstallBindings()
        {
            Container.BindFactory<MovingBlock, MovingBlock.Factory>().
                FromPoolableMemoryPool(x => x.WithInitialSize(20).
                    FromComponentInNewPrefab(_movingBlock).
                    UnderTransformGroup("MovingBlocks"));
            
            
            Container.BindFactory<LevelFinishBlock, LevelFinishBlock.Factory>().
                FromPoolableMemoryPool(x => x.WithInitialSize(2).
                    FromComponentInNewPrefab(_levelFinishBlock).
                    UnderTransformGroup("LevelFinishBlocks"));
            
            Container.BindInstance(_blockPlacementManager).AsSingle();
            Container.BindInstance(_blockPlacementData).AsSingle();
            Container.BindInstance(_playerData).AsSingle();
            Container.BindInstance(_vCam).AsSingle();
            
            Container.Bind<CameraController>().AsSingle();
        }
    }
}