using System.Collections.Generic;
using WheelOfFortune.Modules.RewardSystem.Data;

namespace Modules.WheelOfFortuneSystem.Architecture.Data
{
    internal class WheelOfFortuneModel
    {
        public int CurrentSpinCount { get; private set; }
        public int CurrentRewardIndex { get; private set; }

        public List<Reward> CurrentRewards { get; private set; }

        public void SetCurrentSpinCount(int currentSpinCount)
        {
            CurrentSpinCount = currentSpinCount;
        }

        public void SetCurrentRewardIndex(int currentRewardIndex)
        {
            CurrentRewardIndex = currentRewardIndex;
        }

        public void SetCurrentRewards(List<Reward> currentRewards)
        {
            CurrentRewards = currentRewards;
        }
    }
}