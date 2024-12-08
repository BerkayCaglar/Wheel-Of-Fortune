using Modules.RewardSystem.Enum;
using UnityEngine;
using WheelOfFortune.InventorySystem;
using WheelOfFortune.Modules.EventSystem;
using WheelOfFortune.Modules.RewardSystem.Data;

namespace WheelOfFortune.Modules.GameSystem
{
    public class GameManager : MonoBehaviour
    {
        // Singleton instance
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            // Ensure only one instance of GameManager exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SubscribeToEvents();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            UnSubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventManager.OnSpinWheelResult += OnSpinWheelResult;
            EventManager.OnReviveButtonClicked += OnReviveButtonClicked;
            EventManager.OnGiveUpButtonClicked += OnGiveUpButtonClicked;
        }

        private void UnSubscribeToEvents()
        {
            EventManager.OnSpinWheelResult -= OnSpinWheelResult;
            EventManager.OnReviveButtonClicked -= OnReviveButtonClicked;
            EventManager.OnGiveUpButtonClicked -= OnGiveUpButtonClicked;
        }

        private void OnSpinWheelResult(Reward reward)
        {
            if (reward.Type == ERewardType.Bomb)
            {
                EventManager.InvokeShowYouLosePopup();
            }
        }

        private void OnReviveButtonClicked()
        {
            InventoryManager.RemoveItemAmount(ERewardType.Cash, 200);
            EventManager.InvokeRevivePlayer();
        }

        private void OnGiveUpButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}