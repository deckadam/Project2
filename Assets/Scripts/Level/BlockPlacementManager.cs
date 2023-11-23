using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEditor.PackageManager;
using UnityEngine;
using Zenject;

namespace Level
{
    public class BlockPlacementManager : MonoBehaviour
    {
        private MovingBlock.Factory _movingBlockFactory;
        private BlockPlacementData _blockPlacementData;
        private List<Block> _activeBlocks;
        private MovingBlock _activeBlock;

        private float _currentZ;
        private bool _isRightSide;

        [Inject]
        private void Inject(MovingBlock.Factory movingBlockFactory, BlockPlacementData blockPlacementData)
        {
            _movingBlockFactory = movingBlockFactory;
            _blockPlacementData = blockPlacementData;
        }

        private void OnEnable()
        {
            _activeBlocks = new List<Block>();
            CreateMovingBlock(true);
            EventSystem.Subscribe<BlockPlaceRequestedEvent>(OnBlockPlacementRequested);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<BlockPlaceRequestedEvent>(OnBlockPlacementRequested);
        }

        private void OnBlockPlacementRequested(BaseEvent obj)
        {
            if (_activeBlock == null)
            {
                return;
            }

            _activeBlock.Place();
        }


        public async void StartPlacement()
        {
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            while (!cancellationToken.IsCancellationRequested)
            {
                CreateMovingBlock();
                await UniTask.Delay(_blockPlacementData.BlockSpawnDelay);
            }
        }

        private void CreateMovingBlock(bool isFirstBlock = false)
        {
            var newBlock = _movingBlockFactory.Create();
            newBlock.Initialize(_currentZ, _isRightSide, isFirstBlock);
            _isRightSide = !_isRightSide;
            _activeBlocks.Add(newBlock);
            _currentZ += newBlock.GetSize();
            _activeBlock = newBlock;
        }
    }
}