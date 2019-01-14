using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Factories
{
    public static class QuestFactory
    {
        private static readonly List<Quest> _quest = new List<Quest>();
        static QuestFactory()
        {
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItem = new List<ItemQuantity>();
            itemsToComplete.Add(new ItemQuantity(9002, 5));
            rewardItem.Add(new ItemQuantity(1001, 1));

            _quest.Add(new Quest(1, "Clear the Herb Garden", "Defeat the snakes in the garden and bring 5 Snakeskins back.",
                itemsToComplete, 10, 10, rewardItem));
        }
        internal static Quest GetQuestByID(int id)
        {
            return _quest.FirstOrDefault(a => a.ID == id);
        }
        
    }






}
