using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class Weapon : GameItem
    {
        public int MinimumDamage { get;  }
        public int MaximumDamage { get;  }
        public Weapon (int itemTypeId, string name, int price, int minDamage, int maxDamage) 
            : base (itemTypeId, name, price, true)
        {
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
        }
        public new Weapon Clone()
        {
            return new Weapon(ItemTypeId, Name, Price, MinimumDamage, MaximumDamage);
        }

    }
}
 