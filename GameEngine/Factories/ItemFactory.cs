﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Factories
{
    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();
        static ItemFactory()
        {
           
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 1, 3);
            BuildMiscellaneusItem(9001, "Snake Fang", 1);
            BuildMiscellaneusItem(9002, "Snakeskin", 2);
            BuildMiscellaneusItem(9003, "Rat tail", 1);
            BuildMiscellaneusItem(9004, "Rat fur", 2);
            BuildMiscellaneusItem(9005, "Spider fang", 1);
            BuildMiscellaneusItem(9006, "Spider silk", 2);
            

        }
        public static GameItem CreateGameItem(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(x => x.ItemTypeId == itemTypeID)?.Clone();         
        }
        private static void BuildWeapon( int itemTypeId, string name, int price,
             int minimumDamage = 0, int maximumDamage = 0)
        {
             _standardGameItems.Add(new GameItem
                 (GameItem.ItemCategory.Weapon, itemTypeId, name, price, true, minimumDamage, maximumDamage));
        }
        private static void BuildMiscellaneusItem(int itemTypeId, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, itemTypeId, name, price));
        }

    }
}
