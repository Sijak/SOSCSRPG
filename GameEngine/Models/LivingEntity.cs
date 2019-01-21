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
        #region Properties

        private string _name;
        private int _currentHitPoint;
        private int _maximumHitPoint;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int CurrentHitPoint
        {
            get { return _currentHitPoint; }
            private set
            {
                _currentHitPoint = value;
                OnPropertyChanged();
            }
        }

        public int MaximumHitPoint
        {
            get { return _maximumHitPoint; }
            protected set
            {
                _maximumHitPoint = value;
                OnPropertyChanged();
            }
        }

        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentWeapon = value;
                if (_currentWeapon!=null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                OnPropertyChanged();
            }
        }

        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable!=null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentConsumable = value;
                if(_currentConsumable!=null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameItem> Inventory { get; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }
        public List<GameItem> Weapons => Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();
        public List<GameItem> Consumables => Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();
        public bool HasConsumable => Consumables.Any();
        public bool IsDead => CurrentHitPoint <= 0;

        #endregion

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;


        protected LivingEntity(string name, int currentHitPoint, int maximumHitPoint, int gold, int level=1)
        {
            Name = name;
            CurrentHitPoint = currentHitPoint;
            MaximumHitPoint = maximumHitPoint;
            Gold = gold;
            Level = level;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void UseCurrentWeapon(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoint -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoint = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoint += hitPointsToHeal;
            if(CurrentHitPoint > MaximumHitPoint)
            {
                CurrentHitPoint = MaximumHitPoint;
            }
        }

        public void CompleteHeal()
        {
            CurrentHitPoint = MaximumHitPoint;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold)
        {
            if(Gold<amountOfGold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} Gold, and cannot spend {amountOfGold} Gold.");
            }
            Gold -= amountOfGold;
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);
            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(t => t.Item.ItemTypeId == item.ItemTypeId))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }
                GroupedInventory.First(m => m.Item.ItemTypeId == item.ItemTypeId).Quantity++;
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
           
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);
            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);
            if (item.IsUnique)
            {
                GroupedInventory.Remove(groupedInventoryItemToRemove);
            }
            else
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemsFromInventory (List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity quantity in itemQuantities)
                for (int i=0; i<quantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(t => t.ItemTypeId == quantity.ItemID));
                }
        }

        public bool HasAllItems (List<ItemQuantity> itemsNeeded)
        {
            foreach (ItemQuantity item in itemsNeeded)
            {
                if(Inventory.Count(t=>t.ItemTypeId == item.ItemID)<item.Quantity)
                {
                    return false;
                }               
            }
            return true;
        }
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        private void RaiseActionPerformedEvent (object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
