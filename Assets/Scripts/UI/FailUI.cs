using Cysharp.Threading.Tasks;
using Event;
using Player.Events;
using UnityEngine.SceneManagement;

namespace UI
{
    public class FailUI : UIBase
    {
        private void OnEnable()
        {
            EventSystem.Subscribe<PlayerFallRequestedEvent>(OnGameFailed);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<PlayerFallRequestedEvent>(OnGameFailed);
        }

        private void OnGameFailed(object obj)
        {
            SetVisibility(true, showDelay: _showDelay);
        }

        public async void OnClick()
        {
            await SceneManager.UnloadSceneAsync(1);
            await SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
            SetVisibility(false);
        }
    }
}