using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _movementController;
        private CameraController _cameraController;

        [Inject]
        private void Inject(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        private void OnEnable()
        {
            _cameraController.Initialize(this);
            EventSystem.Subscribe<PlayerFallRequestedEvent>(OnPlayerFallRequested);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<PlayerFallRequestedEvent>(OnPlayerFallRequested);
        }

        private void OnPlayerFallRequested(object obj)
        {
            _movementController.StopMovement();
        }
    }
}