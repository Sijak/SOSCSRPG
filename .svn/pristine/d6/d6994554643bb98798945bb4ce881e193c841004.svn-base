using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Factories;

namespace GameEngine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestAvailableHere { get; set; } = new List<Quest>();//In this way I dont have to make a constructor.

        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();
        public Trader TraderHere { get; set; }
        public void AddMonster(int monsterID, int chanceOfEncounter)
        {
            if (MonstersHere.Exists(m=>m.MonsterID==monsterID))
            {
                MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncounter = chanceOfEncounter;
            }
            else
            {
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncounter));
            }
              
        }

        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }
            int totalChance = MonstersHere.Sum(x => x.ChanceOfEncounter);
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChance);

            int runningTotal = 0;
            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncounter;
                if (randomNumber <= runningTotal)
                {
                    MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
            //if there was a problem, get the monster from the last of list

        }
    }
}
