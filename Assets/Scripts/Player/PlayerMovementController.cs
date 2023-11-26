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
        private PlayerData _movementData;
        private CancellationTokenSource _tokenSource;
        private float _currentCenter;

        [Inject]
        private void Inject(PlayerData playerData)
        {
            _movementData = playerData;
        }

        private void OnEnable()
        {
            EventSystem.Subscribe<GameStartRequestedEvent>(OnGameStartRequested);
            EventSystem.Subscribe<LaneCenterChangedEvent>(OnLaneCenterChanged);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<GameStartRequestedEvent>(OnGameStartRequested);
            EventSystem.Unsubscribe<LaneCenterChangedEvent>(OnLaneCenterChanged);
            StopMovement();
        }

        private void OnLaneCenterChanged(object data)
        {
            var convertedData = data as LaneCenterChangedEvent;
            _currentCenter = convertedData.newLineCenter;
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

                MoveCharacter();

                if (!IsCharacterOnAPlatrform())
                {
                    EventSystem.Raise(new PlayerFallRequestedEvent());
                }
            }
        }

        private void MoveCharacter()
        {
            var currentPosition = transform.localPosition;
            var currentX = currentPosition.x;
            var lerpedValue = Mathf.MoveTowards(currentX, _currentCenter, _movementData.PlayerHorizontalSpeed * Time.deltaTime);
            transform.position = new Vector3(lerpedValue, 0, currentPosition.z + _movementData.PlayerForwardSpeed * Time.deltaTime);
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