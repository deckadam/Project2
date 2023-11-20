using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public async void StartGame()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}