using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class GamePlayUI : MonoBehaviour
    {
        public event Action OnGameStartRequestedEvent;

        [SerializeField] private CanvasGroup _startGameGroup;

        private MainUI _mainUI;

        [Inject]
        private void Inject(MainUI mainUI)
        {
            _mainUI = mainUI;
        }

        public async void OnGoBackClicked()
        {
            await _mainUI.Show();
            await SceneManager.UnloadSceneAsync(1);
        }

        public void OnPlayClicked()
        {
            _startGameGroup.alpha = 0f;
            _startGameGroup.interactable = false;
            _startGameGroup.blocksRaycasts = false;
            Debug.LogError("Game started");
            OnGameStartRequestedEvent?.Invoke();
        }
    }
}