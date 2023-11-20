using Cysharp.Threading.Tasks;
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

        private void Awake()
        {
            _cameraController.Initialize(this);
            StartGameplay();
        }

        public async void StartGameplay()
        {
            _movementController.StartMovement();
            await UniTask.Delay(10000);
            _movementController.StopMovement();
        }
    }
}