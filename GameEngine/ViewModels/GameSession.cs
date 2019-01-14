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
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
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
        public World CurrentWorld { get; set; }
        public Monster CurrentMonster
        {
            get
            {
                return _currentMonster;
            }
            set
            {
                _currentMonster = value;
                OnPropertyChanged(nameof(HasMonster));
                OnPropertyChanged(nameof(CurrentMonster));
                if (CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here.");
                }
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
                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public GameSession()
        {
            CurrentPlayer = new Player
            {
                Name = "SK",
                CharacterClass = "Warrior",
                CurrentHitPoint = 10,
                MaximumHitPoint = 10,
                EXPPoint = 0,
                Level = 1,
                Gold = 1000000
            };

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1001));

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
                CurrentMonster.CurrentHitPoint -= damageToMonster;
                RaiseMessage($"You hit {CurrentMonster.Name} for {damageToMonster} points");
            }
            //if monster is killed, collect xp and loot
            if (CurrentMonster.CurrentHitPoint <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated {CurrentMonster.Name}");

                CurrentPlayer.EXPPoint += CurrentMonster.RewardEXPPoint;
                RaiseMessage($"You receive {CurrentMonster.RewardEXPPoint} experience points.");
                CurrentPlayer.Gold += CurrentMonster.Gold;
                RaiseMessage($"You receive {CurrentMonster.Gold} gold.");
                foreach (GameItem gameItem in CurrentMonster.Inventory)
                {
                    CurrentPlayer.Inventory.Add(gameItem);
                    RaiseMessage($"You obtained one {gameItem.Name}.");
                }
                //get another monster to fight
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
                    CurrentPlayer.CurrentHitPoint -= damageToPlayer;
                    RaiseMessage($"{CurrentMonster.Name} hit you for {damageToPlayer} points.");
                }
                //if player is killed. move to home
                if (CurrentPlayer.CurrentHitPoint <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"{CurrentMonster.Name} has killed you.");
                    CurrentLocation = CurrentWorld.LocationAt(0, -1); //home
                    CurrentPlayer.CurrentHitPoint = CurrentPlayer.Level * 10; //heal the player

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
                        CurrentPlayer.EXPPoint += quest.RewardEXPPoint;
                        RaiseMessage($"You receive {quest.RewardEXPPoint} experience points");

                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            CurrentPlayer.Inventory.Add(rewardItem);
                            RaiseMessage($"You received {rewardItem.Name}");
                        }
                        questToComplete.IsCompleted = true;
                    }
                }
                
            }
        }
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}