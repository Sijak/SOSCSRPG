using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class ItemQuantity
    {
        public int ItemID { get; }
        public int Quantity { get; }
        public ItemQuantity(int itemID, int quantity) //constructor for ItemQuantity class
        {
            ItemID = itemID;
            Quantity = quantity;
        }
    }
}
