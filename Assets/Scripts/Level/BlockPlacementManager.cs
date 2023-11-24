using System.Collections.Generic;
using System.Threading;
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
        private LevelFinishBlock.Factory _finishLineBlockFactory;
        private BlockPlacementData _blockPlacementData;
        private List<Block> _activeBlocks;
        private MovingBlock _activeBlock;
        private CancellationTokenSource _cancellationTokenSource;

        private float _currentZ;
        private bool _isRightSide;

        [Inject]
        private void Inject(MovingBlock.Factory movingBlockFactory, LevelFinishBlock.Factory finishLineBlockFactory, BlockPlacementData blockPlacementData)
        {
            _movingBlockFactory = movingBlockFactory;
            _finishLineBlockFactory = finishLineBlockFactory;
            _blockPlacementData = blockPlacementData;
        }

        private void OnEnable()
        {
            _activeBlocks = new List<Block>();
            CreateMovingBlock(true);
            EventSystem.Subscribe<BlockPlaceRequestedEvent>(OnBlockPlacementRequested);
            EventSystem.Subscribe<PlayerFallRequestedEvent>(OnPlayerFallRequested);
        }


        private void OnDisable()
        {
            EventSystem.Unsubscribe<BlockPlaceRequestedEvent>(OnBlockPlacementRequested);
            EventSystem.Unsubscribe<PlayerFallRequestedEvent>(OnPlayerFallRequested);
        }

        private void OnPlayerFallRequested(object obj)
        {
            CancelPlacement();
        }

        private void CancelPlacement()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
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
            var manualCancellationToken = new CancellationTokenSource();
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(manualCancellationToken.Token, cancellationToken);
            var spawnedBlockCount = 0;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (spawnedBlockCount >= _blockPlacementData.BlockCountPerLevel)
                {
                    Debug.LogError("Level finish check  " + spawnedBlockCount + "  " + _blockPlacementData.BlockCountPerLevel);
                    CreateLevelFinishLine();
                    CancelPlacement();
                    return;
                }
                else
                {
                    _activeBlock = CreateMovingBlock();
                }

                spawnedBlockCount++;
                var isCanceled = await UniTask.Delay(_blockPlacementData.BlockSpawnDelay, cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    return;
                }
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

        private void CreateLevelFinishLine()
        {
            var newBlock = _finishLineBlockFactory.Create();
            newBlock.Initialize(_currentZ, _isRightSide, true);
            _activeBlocks.Add(newBlock);
            _currentZ += newBlock.GetSize();
        }
    }
}