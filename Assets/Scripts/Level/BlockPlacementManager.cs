using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
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

        private void OnBlockPlacementRequested(object obj)
        {
            if (_activeBlock == null)
            {
                return;
            }

            var threshhold = _activeBlock.GetThreshhold();
            var isPerfectPlacement = Mathf.Abs(threshhold) < _blockPlacementData.PlacementThreshold;
            _activeBlock.Place(isPerfectPlacement);
        }

        public async void StartPlacement()
        {
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            while (!cancellationToken.IsCancellationRequested)
            {
                _activeBlock = CreateMovingBlock();
                await UniTask.Delay(_blockPlacementData.BlockSpawnDelay);
            }
        }

        private MovingBlock CreateMovingBlock(bool isFirstBlock = false)
        {
            var newBlock = _movingBlockFactory.Create();
            newBlock.Initialize(_currentZ, _isRightSide, isFirstBlock);
            _isRightSide = !_isRightSide;
            _activeBlocks.Add(newBlock);
            _currentZ += newBlock.GetSize();
            return newBlock;
        }
    }
}