using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;         

namespace GameEngine.Factories
{
    public static class RecipeFactory
    {
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory()
        {
            Recipe granolarBar = new Recipe(1, "Granola bar");
            granolarBar.AddIngredients(3001, 1);
            granolarBar.AddIngredients(3002, 1);
            granolarBar.AddIngredients(3003, 1);
            granolarBar.AddOutputItems(2001, 1);

            _recipes.Add(granolarBar);
        }

        public static Recipe GetRecipeByID(int recipeID)
        {
            return _recipes.FirstOrDefault(t => t.ID == recipeID);
        }

    }
}
