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
        private BlockManagerData _blockManagerData;
        private List<Block> _placedBlocks;
        private Block _activeBlock;
        private CancellationTokenSource _cancellationTokenSource;

        private float _currentZ;
        private bool _isRightSide;
        private bool _isFirstRun = true;
        private CancellationTokenSource _fallToken;

        [Inject]
        private void Inject(MovingBlock.Factory movingBlockFactory, LevelFinishBlock.Factory finishLineBlockFactory, BlockManagerData blockManagerData)
        {
            _movingBlockFactory = movingBlockFactory;
            _finishLineBlockFactory = finishLineBlockFactory;
            _blockManagerData = blockManagerData;
        }

        private void OnEnable()
        {
            _placedBlocks = new List<Block>();
            _placedBlocks.Add(CreateMovingBlock(true));
            _fallToken = new CancellationTokenSource();

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
            _fallToken?.Cancel();
            _fallToken?.Dispose();
            _fallToken = null;
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

            var threshhold = _activeBlock.GetCenter();
            var isPerfectPlacement = Mathf.Abs(threshhold) < _blockManagerData.PlacementThreshold;
            _activeBlock.Place(_placedBlocks[^1], isPerfectPlacement);
            _placedBlocks.Add(_activeBlock);

            if (!isPerfectPlacement)
            {
                DropBlockPiece();
                EventSystem.Raise(new NonPerfectPlacementEvent());
            }
            else
            {
                EventSystem.Raise(new PerfectPlacementEvent());
            }
        }

        private async void DropBlockPiece()
        {
            var dropBlock = CreateMovingBlock(true, true);
            await dropBlock.Drop(_placedBlocks[^1], _placedBlocks[^2]);
            dropBlock.Despawn(0, new CancellationToken());
        }

        public async void StartPlacement()
        {
            if (!_isFirstRun)
            {
                DespawnPreviousBlocks();
            }

            _isFirstRun = false;

            CreateNewCancellationToken();

            var spawnedBlockCount = 0;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (spawnedBlockCount >= _blockManagerData.BlockCountPerLevel)
                {
                    CreateLevelFinishLine();
                    CancelPlacement();
                    return;
                }
                else
                {
                    CreateMovingBlock();
                }

                spawnedBlockCount++;
                var isCanceled = await UniTask.Delay(_blockManagerData.BlockSpawnDelay, cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    return;
                }
            }
        }

        private void CreateNewCancellationToken()
        {
            var manualCancellationToken = new CancellationTokenSource();
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(manualCancellationToken.Token, cancellationToken);
        }

        private void DespawnPreviousBlocks()
        {
            for (var i = 0; i < _placedBlocks.Count - 1; i++)
            {
                var activeBlock = _placedBlocks[i];
                activeBlock.Despawn(_blockManagerData.BlockDespawnDelay * i, _fallToken.Token);
            }

            var lastItem = _placedBlocks[^1];
            _placedBlocks.Clear();
            _placedBlocks.Add(lastItem);
        }

        private Block CreateMovingBlock(bool isLocked = false, bool isDropBlock = false)
        {
            var newBlock = _movingBlockFactory.Create();
            InitializeBlock(newBlock, isLocked, isDropBlock);
            return newBlock;
        }

        private void CreateLevelFinishLine(bool isLocked = false, bool isDropBlock = false)
        {
            var newBlock = _finishLineBlockFactory.Create();
            InitializeBlock(newBlock, isLocked, isDropBlock);
        }

        private void InitializeBlock(Block newBlock, bool isLocked, bool isDropBlock)
        {
            newBlock.Initialize(_currentZ, _isRightSide, isLocked, _activeBlock);

            if (isDropBlock)
            {
                return;
            }

            _isRightSide = !_isRightSide;
            _currentZ += newBlock.GetLength();
            _activeBlock = newBlock;
        }
    }
}