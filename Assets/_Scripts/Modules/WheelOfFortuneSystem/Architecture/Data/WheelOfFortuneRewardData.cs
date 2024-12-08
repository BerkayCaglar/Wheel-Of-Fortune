using Modules.RewardSystem.Enum;
using UnityEngine;

namespace Modules.WheelOfFortuneSystem.Architecture.Data
{
    [CreateAssetMenu(fileName = "RewardData", menuName = "ScriptableObjects/WOF/RewardData", order = 1)]
    public class WheelOfFortuneRewardData : ScriptableObject
    {
        [SerializeField] private string _rewardName;
        [SerializeField] private Sprite _rewardImage;
        [SerializeField] private ERewardType _rewardType;

        public string RewardName => _rewardName;
        public Sprite RewardImage => _rewardImage;
        public ERewardType RewardType => _rewardType;
    }
}