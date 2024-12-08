using System;
using Modules.RewardSystem.Enum;
using UnityEngine;

namespace WheelOfFortune.Modules.RewardSystem.Data
{
    [Serializable]
    public class Reward
    {
        public string Name;
        public int Amount;
        public ERewardType Type;
        public Sprite Sprite;

        public Reward(string name, int amount, ERewardType type, Sprite sprite)
        {
            Name = name;
            Amount = amount;
            Type = type;
            Sprite = sprite;
        }
    }
}