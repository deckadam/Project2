using System.Threading;
using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerData.PlayerMovementData _movementData;
        private CancellationTokenSource _tokenSource;

        [Inject]
        private void Inject(PlayerData playerData)
        {
            _movementData = playerData.MovementData;
        }

        private void OnEnable()
        {
            EventSystem.Subscribe<GameStartRequestedEvent>(OnGameStartRequested);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<GameStartRequestedEvent>(OnGameStartRequested);
            StopMovement();
        }

        private void OnGameStartRequested(object obj)
        {
            StartMovement();
        }

        public async void StartMovement()
        {
            _tokenSource = new CancellationTokenSource();
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_tokenSource.Token, cancellationToken);

            while (!linkedToken.IsCancellationRequested)
            {
                var isCanceled = await UniTask.NextFrame(_tokenSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    return;
                }

                transform.Translate(0, 0, _movementData.PlayerMovementSpeed * Time.deltaTime);

                if (!IsCharacterOnAPlatrform())
                {
                    EventSystem.Raise(new PlayerFallRequestedEvent());
                }
            }
        }

        private bool IsCharacterOnAPlatrform()
        {
            return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 10f, 1 << 6);
        }

        public void StopMovement()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
        }
    }
}