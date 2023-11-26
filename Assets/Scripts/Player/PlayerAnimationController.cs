using Event;
using Player.Events;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int Run = Animator.StringToHash("Run");

        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            EventSystem.Subscribe<GameStartRequestedEvent>(OnGameStartRequested);
            EventSystem.Subscribe<FinishLineReachedEvent>(DanceAnimationRequested);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<GameStartRequestedEvent>(OnGameStartRequested);
            EventSystem.Unsubscribe<FinishLineReachedEvent>(DanceAnimationRequested);
        }

        private void DanceAnimationRequested(object obj)
        {
            _animator.ResetTrigger(Run);
            _animator.SetTrigger(Dance);
        }

        private void OnGameStartRequested(object obj)
        {
            _animator.ResetTrigger(Dance);
            _animator.SetTrigger(Run);
        }
    }
}