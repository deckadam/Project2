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
            EventSystem.Subscribe<FinishLineReachedEvent>(OnFinishLineReached);
        }

        private void OnDisable()
        {
            _cameraController.Deinitialize();

            EventSystem.Unsubscribe<PlayerFallRequestedEvent>(OnPlayerFallRequested);
            EventSystem.Unsubscribe<FinishLineReachedEvent>(OnFinishLineReached);
        }


        private void OnFinishLineReached(object obj)
        {
            _movementController.StopMovement();
        }

        private void OnPlayerFallRequested(object obj)
        {
            _movementController.StopMovement();
        }
    }
}