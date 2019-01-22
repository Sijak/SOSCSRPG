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
                    CurrentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    CurrentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    CurrentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    
                }
                _currentPlayer = value;
                if (_currentPlayer != null)
                {
                    CurrentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
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
                    CurrentMonster.OnActionPerformed -= OnCurrentMonsterPerformedAction;
                    CurrentMonster.OnKilled -= OnCurrentMonsterKilled;
                }
                _currentMonster = value;
                if(_currentMonster!=null)
                {
                    CurrentMonster.OnActionPerformed += OnCurrentMonsterPerformedAction;
                    CurrentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"You have {CurrentMonster.Name} here!");
                }
                OnPropertyChanged(nameof(HasMonster));
                OnPropertyChanged();
                
            }
        }

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
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.LearnRecipe(RecipeFactory.GetRecipeByID(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));


        }
        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.X, CurrentLocation.Y + 1) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.X, CurrentLocation.Y - 1) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.X + 1, CurrentLocation.Y) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.X - 1, CurrentLocation.Y) != null;

        public void MoveNorth()
        {
            if (HasLocationToNorth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.X, CurrentLocation.Y + 1);
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.X, CurrentLocation.Y - 1);
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.X - 1, CurrentLocation.Y);
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.X + 1, CurrentLocation.Y);
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
            if (CurrentMonster == null)
            { return; }

            if (CurrentPlayer.CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon to attack.");
                return;
            }
            //Determine damage to monsters
            CurrentPlayer.UseCurrentWeapon(CurrentMonster);

            //if monster is killed, collect xp and loot
            if (CurrentMonster.IsDead)
            {              
                GetMonsterAtLocaion();
            }
            else
            {
                //if monster is still alive, monster attacks you
                CurrentMonster.UseCurrentWeapon(CurrentPlayer);
                
            }
        }
        public void CompleteQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID);
                if (questToComplete!=null)
                {
                    if(CurrentPlayer.HasAllItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);
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

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable!=null)
            {
                CurrentPlayer.UseCurrentConsumable();
            }
        }

        public void CraftItemUsing (Recipe recipe)
        {
            if (CurrentPlayer.HasAllItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);
                foreach (ItemQuantity itemsNeeded in recipe.OutputItems)
                {
                    for(int i=0;i<itemsNeeded.Quantity;i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemsNeeded.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        RaiseMessage($"You crafted one {outputItem.Name}");
                    }
                }
            }
            else
            {
                RaiseMessage($"You don't have required ingredients.");
                foreach (ItemQuantity itemsNeeded in recipe.Ingredients)
                {
                    RaiseMessage($"{itemsNeeded.Quantity} {ItemFactory.ItemName(itemsNeeded.ItemID)}");
                }
            }
        }

        private void OnCurrentPlayerPerformedAction (object sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentMonsterPerformedAction (object sender, string result)
        {
            RaiseMessage(result);
        }
        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You Died");
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