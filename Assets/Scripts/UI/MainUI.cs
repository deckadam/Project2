using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainUI : UIBase
    {
        public async void StartGame()
        {
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            SetVisibility(false);
        }

        public UniTask Show()
        {
            return SetVisibility(true);
        }
    }
}