using Modules.ScriptableSystem;
using NaughtyAttributes;
using UnityEngine;
using WheelOfFortune.InventorySystem;

namespace WheelOfFortune.Modules.ApplicationSystem
{
    public class ApplicationManager : MonoBehaviour
    {
        [Button("Start")]
        private async void Start()
        {
            await ScriptableAPI.Init();
            InventoryManager.Init();
        }

        private void OnApplicationQuit()
        {
            ScriptableAPI.Dispose();
            InventoryManager.Dispose();
        }
    }
}