using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Action;

namespace GameEngine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Weapon,
            Miscellaneous,
            Consumable
        }
        public ItemCategory Category { get; }
        public int ItemTypeId { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }
        public IAction Action { get; set; }
        

        public GameItem(ItemCategory category, int itemTypeId, string name, int price,
            bool isUnique=false, IAction action=null)
        {
            Category = category;
            ItemTypeId = itemTypeId;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
            
        }

        public void PerformAction (LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor,target);
        }
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeId, Name, Price, IsUnique, Action);
        }
    }
}
