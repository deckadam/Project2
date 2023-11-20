using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _movementController;

        private void Awake()
        {
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