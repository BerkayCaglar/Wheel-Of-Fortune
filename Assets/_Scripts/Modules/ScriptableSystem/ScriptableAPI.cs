using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.AddressableSystem;
using Modules.AddressableSystem.Architecture.Data;
using Modules.RewardSystem.Enum;
using Modules.WheelOfFortuneSystem.Architecture.Data;
using UnityEngine;
using WheelOfFortuneSystem.Architecture.Enum;

namespace Modules.ScriptableSystem
{
    public class ScriptableAPI
    {
        private static List<WheelOfFortuneRewardData> _wofRewards;

        public static async UniTask Init()
        {
            _wofRewards = await AddressablesManager.LoadAssets<WheelOfFortuneRewardData>(AddressableConstants.WHEEL_OF_FORTUNE_REWARDS);
        }

        public static void Dispose()
        {
            _wofRewards = null;
        }

        public static List<WheelOfFortuneRewardData> GetUniqueRandomWOFRewards(int count, EWOFType wofType)
        {
            List<WheelOfFortuneRewardData> rewards = new();
            for (int i = 0; i < count; i++)
            {
                WheelOfFortuneRewardData reward = GetRandomReward(wofType);
                if (reward.RewardType != ERewardType.Cash && reward.RewardType != ERewardType.Gold)
                {
                    if (rewards.Any(r => r.RewardType == reward.RewardType))
                    {
                        i--;
                        continue;
                    }
                }

                rewards.Add(reward);
            }

            if (wofType == EWOFType.Bronze && !rewards.Any(r => r.RewardType == ERewardType.Bomb))
            {
                rewards[Random.Range(0, rewards.Count)] = _wofRewards.First(r => r.RewardType == ERewardType.Bomb);
            }

            return rewards;
        }

        private static WheelOfFortuneRewardData GetRandomReward(EWOFType wofType)
        {
            float randomValue = Random.value;
            if (wofType == EWOFType.Bronze)
            {
                if (randomValue < 0.5f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Cash);
                }
                else if (randomValue < 0.8f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Gold);
                }
                else
                {
                    var exlusives = _wofRewards.Where(r => r.RewardType == ERewardType.Gun || r.RewardType == ERewardType.Helmet || r.RewardType == ERewardType.Knife || r.RewardType == ERewardType.UsableItem).ToList();
                    return exlusives[Random.Range(0, exlusives.Count)];
                }
            }
            else if (wofType == EWOFType.Silver)
            {
                if (randomValue < 0.3f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Cash);
                }
                else if (randomValue < 0.6f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Gold);
                }
                else
                {
                    var exlusives = _wofRewards.Where(r => r.RewardType == ERewardType.Gun || r.RewardType == ERewardType.Helmet || r.RewardType == ERewardType.Knife || r.RewardType == ERewardType.UsableItem).ToList();
                    return exlusives[Random.Range(0, exlusives.Count)];
                }
            }
            else
            {
                if (randomValue < 0.2f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Cash);
                }
                else if (randomValue < 0.4f)
                {
                    return _wofRewards.First(r => r.RewardType == ERewardType.Gold);
                }
                else
                {
                    var exlusives = _wofRewards.Where(r => r.RewardType == ERewardType.Gun || r.RewardType == ERewardType.Helmet || r.RewardType == ERewardType.Knife || r.RewardType == ERewardType.UsableItem).ToList();
                    return exlusives[Random.Range(0, exlusives.Count)];
                }
            }
        }
    }
}