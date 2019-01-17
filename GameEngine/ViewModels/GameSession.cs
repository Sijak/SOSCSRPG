using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Factories;
using GameEngine.EventArgs;

namespace GameEngine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        private Trader _currentTrader;
        private Monster _currentMonster;
        private Location _currentLocation;
        private Player _currentPlayer;

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    CurrentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    CurrentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    
                }
                _currentPlayer = value;
                if (_currentPlayer != null)
                {
                    CurrentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    CurrentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));
                CurrentTrader = CurrentLocation.TraderHere;
                CompleteQuestAtLocation();
                GivePlayerQuestAtLocation();
                GetMonsterAtLocaion();
            }
        }
        public World CurrentWorld { get; }
        public Monster CurrentMonster
        {
            get
            {
                return _currentMonster;
            }
            set
            {
                if(_currentMonster!=null)
                {
                    CurrentMonster.OnKilled -= OnCurrentMonsterKilled;
                }
                _currentMonster = value;
                if(_currentMonster!=null)
                {
                    CurrentMonster.OnKilled += OnCurrentMonsterKilled;
                }
                OnPropertyChanged(nameof(HasMonster));
                OnPropertyChanged();
                
            }
        }
        public Weapon CurrentWeapon { get; set; }

        public Trader CurrentTrader
        {
            get
            {
                return _currentTrader;
            }
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public GameSession()
        {
            CurrentPlayer = new Player("SK", "Warrior", 0, 10, 10, 1000000);

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            

        }
        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        public void MoveNorth()
        {
            if (HasLocationToNorth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
        }

        private void GivePlayerQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    RaiseMessage("");
                    RaiseMessage($"You received {quest.Name} quest.");
                    RaiseMessage(quest.Description);
                    RaiseMessage("Return with");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"{itemQuantity.Quantity} pieces of {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    RaiseMessage("You will receive");
                    RaiseMessage($"{quest.RewardEXPPoint} EXP point and {quest.RewardGold} gold.");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID)}");
                    }

                }

            }
        }
        public void GetMonsterAtLocaion()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }
        public void AttackCurrentMonster()
        {
            //guard clause, or early exit
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon to attack.");
                return;
            }
            //Determine damage to monsters
            int damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}");
            }
            else
            {
                RaiseMessage($"You hit {CurrentMonster.Name} for {damageToMonster} points");
                CurrentMonster.TakeDamage(damageToMonster);
                
            }
            //if monster is killed, collect xp and loot
            if (CurrentMonster.IsDead)
            {              
                GetMonsterAtLocaion();
            }
            else
            {
                //if monster is still alive, monster attacks you

                int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage($"{CurrentMonster.Name} misses the attack.");
                }
                else
                {
                    RaiseMessage($"{CurrentMonster.Name} hit you for {damageToPlayer} points.");
                    CurrentPlayer.TakeDamage(damageToPlayer);
                    
                }
                
            }
        }
        public void CompleteQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID);
                if (questToComplete!=null)
                {
                    if(CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i=0; i<itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeId == itemQuantity.ItemID));
                            }
                        }
                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");

                        // Give the player the quest rewards
                        RaiseMessage($"You receive {quest.RewardEXPPoint} experience points");
                        CurrentPlayer.AddEXPPoint(quest.RewardEXPPoint);

                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);
                        
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            RaiseMessage($"You received {rewardItem.Name}");
                            CurrentPlayer.Inventory.Add(rewardItem);
                            
                        }
                        questToComplete.IsCompleted = true;
                    }
                }
                
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"{CurrentMonster.Name} Killed you.aa");
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompleteHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You killed {CurrentMonster.Name}.");

            RaiseMessage($"You received {CurrentMonster.Gold} gold");
            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);

            RaiseMessage($"You received {CurrentMonster.RewardEXPPoint} points of EXP.");
            CurrentPlayer.AddEXPPoint(CurrentMonster.RewardEXPPoint);

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"You received one {gameItem.Name}");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnCurrentPlayerLeveledUp (object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"You are now Level {CurrentPlayer.Level}!");
        }
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}