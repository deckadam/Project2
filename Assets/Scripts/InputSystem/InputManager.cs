using System.Threading;
using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;

        private void OnEnable()
        {
            EventSystem.Subscribe<GameStartRequestedEvent>(CheckForInput);
            EventSystem.Subscribe<FinishLineReachedEvent>(StopCheckingForInput);
            EventSystem.Subscribe<PlayerFallRequestedEvent>(StopCheckingForInput);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<GameStartRequestedEvent>(CheckForInput);
            EventSystem.Unsubscribe<FinishLineReachedEvent>(StopCheckingForInput);
            EventSystem.Unsubscribe<PlayerFallRequestedEvent>(StopCheckingForInput);
        }

        private void StopCheckingForInput(object obj)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async void CheckForInput(object data)
        {
            var manulaToken = new CancellationTokenSource();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(manulaToken.Token, gameObject.GetCancellationTokenOnDestroy());
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EventSystem.Raise(new BlockPlaceRequestedEvent());
                }

                var isCanceled = await UniTask.NextFrame(cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    return;
                }
            }
        }
    }
}