using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        public async void StartGame()
        {
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            await _canvasGroup.DOFade(0f, _fadeDuration).AsyncWaitForCompletion();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public Task Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            return _canvasGroup.DOFade(1f, _fadeDuration).AsyncWaitForCompletion();
        }
    }
}