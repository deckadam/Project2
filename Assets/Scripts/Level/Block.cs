using System.Threading;
using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEngine;
using Utility;
using Zenject;

namespace Level
{
    public class Block : MonoBehaviour
    {
        [SerializeField] protected float _size;
        [SerializeField] private float _movementSpeed;

        protected bool _isOnRightSize;
        protected bool _isLockedIn;

        private CancellationTokenSource _cancellationTokenSource;
        private IMemoryPool _pool;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public async void Despawn(int delay,CancellationToken token)
        {
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, token);
            var isCanceled = await UniTask.Delay(delay, cancellationToken: linkedToken.Token).SuppressCancellationThrow();
            if (isCanceled)
            {
                return;
            }

            _pool.Despawn(this);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
        }


        public void Initialize(float z, bool isOnRightSide, bool isLockedIn, Block previousBlock)
        {
            if (previousBlock != null)
            {
                AdjustScale(previousBlock);
            }

            _isOnRightSize = isOnRightSide;
            _isLockedIn = isLockedIn;

            var xValue = 0f;
            if (!isLockedIn)
            {
                xValue = isOnRightSide ? -10f : 10f;
            }

            transform.position = new Vector3(xValue, 0, z);

            OnInitialize();
        }

        public void Place(Block previousBlock, bool isPerfectPlacement)
        {
            _cancellationTokenSource?.Cancel();
            if (isPerfectPlacement)
            {
                transform.position = transform.position.ChangeX(previousBlock.GetCenter());
            }
            else
            {
                CutBlock(previousBlock);
                EventSystem.Raise(new LaneCenterChangedEvent(GetCenter()));
            }
        }

        private void CutBlock(Block previousBlock)
        {
            var currentScale = transform.localScale;
            var currentPosition = transform.position;
            var difference = previousBlock.GetCenter() - GetCenter();
            var absDifference = Mathf.Abs(difference);
            var scaleDifference = currentScale.x - absDifference;
            transform.localScale = currentScale.ChangeX(scaleDifference);
            transform.position = currentPosition.ChangeX(currentPosition.x + difference / 2f);
        }

        private void AdjustScale(Block previousBlock)
        {
            transform.localScale = previousBlock.transform.localScale;
        }

        private async void OnInitialize()
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

        public float GetWidth() => transform.localScale.x;
        public float GetCenter() => transform.position.x;
        public float GetLength() => _size;
    }
}