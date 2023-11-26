using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class GamePlayUI : UIBase
    {
        private MainUI _mainUI;

        [Inject]
        private void Inject(MainUI mainUI)
        {
            _mainUI = mainUI;
        }

        private void OnEnable()
        {
            EventSystem.Subscribe<FinishLineReachedEvent>(OnLevelFinished);
        }


        private void OnDisable()
        {
            EventSystem.Unsubscribe<FinishLineReachedEvent>(OnLevelFinished);
        }

        private void OnLevelFinished(object obj)
        {
            SetVisibility(true);
        }

        public async void OnGoBackClicked()
        {
            await _mainUI.Show();
            await SceneManager.UnloadSceneAsync(1);
        }

        public void OnPlayClicked()
        {
            SetVisibility(false);
            EventSystem.Raise(new GameStartRequestedEvent());
        }
    }
}