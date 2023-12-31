using Audio;
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
        [SerializeField] private BlockManagerData blockManagerData;
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private MovingBlock _movingBlock;
        [SerializeField] private LevelFinishBlock _levelFinishBlock;
        [SerializeField] private AudioData _audioData;
        [SerializeField] private AudioSource _audioSource;

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
            Container.BindInstance(blockManagerData).AsSingle();
            Container.BindInstance(_playerData).AsSingle();
            Container.BindInstance(_vCam).AsSingle();
            Container.BindInstance(_audioData).AsSingle();
            Container.BindInstance(_audioSource).AsSingle();
            
            Container.Bind<CameraController>().AsSingle();
        }
    }
}