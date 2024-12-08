using Cysharp.Threading.Tasks;
using Modules.ScriptableSystem;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using WheelOfFortune.InventorySystem;

namespace WheelOfFortune.Modules.ApplicationSystem
{
    public class ApplicationManager : MonoBehaviour
    {
        [Button("Start")]
        private async void Start()
        {
            Application.targetFrameRate = 60;
            await ScriptableAPI.Init();
            InventoryManager.Init();
            await UniTask.Delay(5000);
            await SceneManager.LoadSceneAsync(1);
        }

        private void OnApplicationQuit()
        {
            ScriptableAPI.Dispose();
            InventoryManager.Dispose();
        }
    }
}