using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Weapon,
            Miscellaneous       
        }
        public ItemCategory Category { get; }
        public int ItemTypeId { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }
        public int MinimumDamage { get; }
        public int MaximumDamage { get; }

        public GameItem(ItemCategory category, int itemTypeId, string name, int price,
            bool isUnique=false, int minimumDamage=0, int maximumDamage=0)
        {
            Category = category;
            ItemTypeId = itemTypeId;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            MaximumDamage = maximumDamage;
            MinimumDamage = minimumDamage;
        }
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeId, Name, Price, IsUnique,MinimumDamage,MaximumDamage);
        }
    }
}
