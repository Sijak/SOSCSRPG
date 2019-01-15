using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace GameEngine.Models
{
    public class Player : LivingEntity

    {
        

        private int _eXPPoint;
        private string _characterClass;         
        private int _level;
  
        public string CharacterClass
        {
            get
            {
                return _characterClass;
            }
            set
            {
                _characterClass = value;
                OnPropertyChanged(nameof(CharacterClass));
            }
        }
        
        public int EXPPoint
        {
            get
            {
                return _eXPPoint;
            }
            set
            {
                _eXPPoint = value;
                OnPropertyChanged(nameof(EXPPoint));
            }
        }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }

        }
       
        
        public ObservableCollection<QuestStatus> Quests { get; set; }

        public Player(string name, string characterClass, int eXPPoint, int level, int currentHitPoint, int maximumHitPoint, int gold)
            :base(name, currentHitPoint,maximumHitPoint,gold)
        {
            CharacterClass = characterClass;
            EXPPoint = eXPPoint;
            Level = level;
            Quests = new ObservableCollection<QuestStatus>();
        }
        
        public bool HasAllTheseItems (List<ItemQuantity> xoxo )
        {
            foreach (ItemQuantity item in xoxo)
            {
                if (Inventory.Count(i => i.ItemTypeId == item.ItemID)<item.Quantity)
                { return false; }
                
            }
            return true;
            
        }
    }
    

}
