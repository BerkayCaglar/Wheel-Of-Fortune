using System;
using System.Collections.Generic;
using WheelOfFortune.Modules.RewardSystem.Data;

namespace WheelOfFortune.Modules.EventSystem
{
    public class EventManager
    {
        public static Action<Reward> OnSpinWheelResult;

        public static Action OnShowYouLosePopup;
        public static Action OnReviveButtonClicked;
        public static Action OnGiveUpButtonClicked;

        public static Action OnRevivePlayer;

        public static void InvokeSpinWheelResult(Reward reward)
        {
            OnSpinWheelResult?.Invoke(reward);
        }

        public static void InvokeShowYouLosePopup()
        {
            OnShowYouLosePopup?.Invoke();
        }

        public static void InvokeReviveButtonClicked()
        {
            OnReviveButtonClicked?.Invoke();
        }

        public static void InvokeGiveUpButtonClicked()
        {
            OnGiveUpButtonClicked?.Invoke();
        }

        public static void InvokeRevivePlayer()
        {
            OnRevivePlayer?.Invoke();
        }
    }
}