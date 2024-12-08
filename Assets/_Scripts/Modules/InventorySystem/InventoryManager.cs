using System;
using System.Collections.Generic;
using System.Linq;
using Modules.RewardSystem.Enum;
using WheelOfFortune.Modules.EventSystem;
using WheelOfFortune.Modules.RewardSystem.Data;

namespace WheelOfFortune.InventorySystem
{
    public class InventoryManager
    {
        private static readonly List<Reward> Items = new();

        public static void Init()
        {
            EventManager.OnSpinWheelResult += AddItem;
        }

        public static void Dispose()
        {
            EventManager.OnSpinWheelResult -= AddItem;
        }

        public static void AddItem(Reward item)
        {
            if (Items.Any(x => x.Type == item.Type))
            {
                Items.Find(x => x.Type == item.Type).Amount += item.Amount;
            }
            else
            {
                Items.Add(item);
            }
        }

        public static void RemoveItemAmount(ERewardType type, int amount)
        {
            if (Items.Any(x => x.Type == type))
            {
                Items.Find(x => x.Type == type).Amount -= amount;
            }
        }

        public static bool HasAmount(ERewardType type, int amount)
        {
            return Items.Any(x => x.Type == type && x.Amount >= amount);
        }

        public static void ClearItems()
        {
            Items.Clear();
        }

        public static List<Reward> GetItems()
        {
            return Items;
        }

        public static List<Reward> GetItems(ERewardType type)
        {
            return Items.Where(x => x.Type == type).ToList();
        }
    }
}