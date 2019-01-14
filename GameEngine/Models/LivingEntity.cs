using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
        //as an abstract class, LivingEntity cannot be instantiated. Child can be.
    {
        private string _name;
        private int _currentHitPoint;
        private int _maximumHitPoint;
        private int _gold;

        public string Name
        {
            get {return _name;}
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int CurrentHitPoint
        {
            get { return _currentHitPoint; }
            set
            {
                _currentHitPoint = value;
                OnPropertyChanged(nameof(CurrentHitPoint));
            }
        }

        public int MaximumHitPoint
        {
            get { return _maximumHitPoint; }
            set
            {
                _maximumHitPoint = value;
                OnPropertyChanged(nameof(MaximumHitPoint));
            }
        }

        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged(nameof(Gold));
            }
        }

        public ObservableCollection<GameItem> Inventory { get; set; }

        public List<GameItem> Weapons => Inventory.Where(i => i is Weapon).ToList();

        protected LivingEntity()
        {
            Inventory = new ObservableCollection<GameItem>();
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);
            OnPropertyChanged(nameof(Weapons));
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);
            OnPropertyChanged(nameof(Weapons));
        }
    }
}
