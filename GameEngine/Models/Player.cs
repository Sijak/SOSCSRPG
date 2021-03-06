﻿using System;
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


        public string CharacterClass
        {
            get
            {
                return _characterClass;
            }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int EXPPoint
        {
            get
            {
                return _eXPPoint;
            }
            private set
            {
                _eXPPoint = value;
                SetLevelAndMaximumHitPoint();
                OnPropertyChanged();
               
            }
        }

        public event EventHandler OnLeveledUp;

        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Recipe> Recipes { get; }

        public Player(string name, string characterClass, int eXPPoint, int currentHitPoint, int maximumHitPoint, int gold)
            : base(name, currentHitPoint, maximumHitPoint, gold)
        {
            CharacterClass = characterClass;
            EXPPoint = eXPPoint;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

      

        public void LearnRecipe (Recipe recipe)
        {
            if (!Recipes.Any(t=>t.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        public void AddEXPPoint (int eXPPoint)
        {
            EXPPoint += eXPPoint;
        }
        private void SetLevelAndMaximumHitPoint()
        {
            int originalLevel = Level;
            Level = (EXPPoint / 10) + 1;
            if(Level!=originalLevel)
            {
                MaximumHitPoint = Level * 10;
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }

        }
    }
    

}
