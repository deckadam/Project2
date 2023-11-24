using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private BlockPlacementManager _blockPlacementManager;

        [Inject]
        private void Inject(BlockPlacementManager blockPlacementManager)
        {
            _blockPlacementManager = blockPlacementManager;
        }

        private void OnEnable()
        {
            EventSystem.Subscribe<GameStartRequestedEvent>(StartGame);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<GameStartRequestedEvent>(StartGame);
        }

        private void StartGame(object obj)
        {
            _blockPlacementManager.StartPlacement();
        }
    }
}