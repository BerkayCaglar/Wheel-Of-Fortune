using System.Collections.Generic;
using Modules.RewardSystem.Enum;
using Modules.ScriptableSystem;
using UnityEngine;
using WheelOfFortune.Modules.RewardSystem.Data;
using WheelOfFortuneSystem.Architecture.Enum;

namespace Modules.WheelOfFortuneSystem.Managers
{
    public class WheelOfFortuneManager
    {
        internal static List<Reward> CalculateRewards(int spinCount, int sliceCount, EWOFType wOFType)
        {
            var rewards = ScriptableAPI.GetUniqueRandomWOFRewards(sliceCount, wOFType);
            var result = new List<Reward>();
            spinCount = spinCount == 0 ? 1 : spinCount;
            for (int i = 0; i < sliceCount; i++)
            {
                var reward = rewards[i];
                var amount = reward.RewardType switch
                {
                    ERewardType.Cash => Random.Range(100, 1000000) * Mathf.Pow(spinCount, 2) / 2,
                    ERewardType.Gold => Random.Range(10, 10000) * Mathf.Pow(spinCount, 2) / 2,
                    _ => 1
                };
                result.Add(new Reward(reward.RewardName, (int)amount, reward.RewardType, reward.RewardImage));
            }

            return result;
        }

        internal static EWOFType GetWOFType(int spinCount)
        {
            if (spinCount == 0) { return EWOFType.Bronze; }
            return spinCount switch
            {
                int n when n % 5 == 0 => EWOFType.Silver,
                int n when n % 30 == 0 => EWOFType.Gold,
                _ => EWOFType.Bronze
            };
        }
    }
}