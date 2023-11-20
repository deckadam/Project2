using System.Threading;
using Cysharp.Threading.Tasks;
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

        private void OnDisable()
        {
            StopMovement();
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
            }
        }

        public void StopMovement()
        {
            _tokenSource.Cancel();
        }
    }
}