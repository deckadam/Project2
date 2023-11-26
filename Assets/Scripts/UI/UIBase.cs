using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] protected float _showDelay;
        
        [SerializeField] private bool _isVisibleAtStart;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (!_isVisibleAtStart)
            {
                SetVisibility(false, true);
            }
        }

        protected UniTask SetVisibility(bool isVisible, bool immediate = false, float showDelay = 0f)
        {
            var valueToFade = isVisible ? 1f : 0f;
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
            if (immediate)
            {
                _canvasGroup.alpha = 0f;
                return UniTask.CompletedTask;
            }

            return _canvasGroup.DOFade(valueToFade, _fadeDuration).SetDelay(showDelay).AsyncWaitForCompletion().AsUniTask();
        }
    }
}