using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Level
{
    public class MovingBlock : Block, IPoolable<IMemoryPool>
    {
        [SerializeField] private float _movementSpeed;

        private CancellationTokenSource _cancellationTokenSource;

        protected override async void OnInitialize()
        {
            if (_isLockedIn)
            {
                return;
            }

            var manualCancellationTokenSource = new CancellationTokenSource();
            var onDestroyCancellationToken = gameObject.GetCancellationTokenOnDestroy();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(manualCancellationTokenSource.Token, onDestroyCancellationToken);

            var movementValue = _isOnRightSize ? _movementSpeed : -_movementSpeed;
            while (!onDestroyCancellationToken.IsCancellationRequested)
            {
                transform.Translate(movementValue * Time.deltaTime, 0, 0);

                var isCanceled = await UniTask.NextFrame(_cancellationTokenSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    break;
                }
            }
        }

        public float GetThreshhold()
        {
            return transform.position.x;
        }

        public void Place(bool isPerfectPlacement)
        {
            _cancellationTokenSource?.Cancel();
            if (isPerfectPlacement)
            {
                transform.position = transform.position.ChangeX(0);
            }
        }

        public class Factory : PlaceholderFactory<MovingBlock> { }
    }
}