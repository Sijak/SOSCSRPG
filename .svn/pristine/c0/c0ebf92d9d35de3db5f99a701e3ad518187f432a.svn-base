using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(0, -1, "Home",
                "This is your great Home.",
                "Home.jpg");
            newWorld.AddLocation(1, 0, "Town Gate",
                "You are at the eastern gate of the town.",
                "TownGate.PNG");
            newWorld.AddLocation(0, 0, "Town Square",
                "You are in the middle of Town.",
                "TownSquare.PNG");
            newWorld.AddLocation(2, 0, "Spider Forest",
                "Danger is nearby.",
                "SpiderForest.PNG");
            newWorld.LocationAt(2, 0).AddMonster(3, 100);
            newWorld.AddLocation(0, 1, "Herbalist's Hut",
                "Maybe good Herb is on sale?",
                "HerbalistsHut.PNG");
            newWorld.LocationAt(0, 1).TraderHere = TraderFactory.GetTraderByName("Pete the Herbalist");
            newWorld.LocationAt(0, 1).QuestAvailableHere.Add(QuestFactory.GetQuestByID(1));
            newWorld.AddLocation(0, 2, "Herbarlist's Garden",
                "Be careful of snakes.",
                "HerbalistsGarden.PNG");
            newWorld.LocationAt(0, 2).AddMonster(1, 100);
            newWorld.AddLocation(-1, -1, "Farm House",
                "There might be a quest for you here.",
                "Farmhouse.PNG");
            newWorld.LocationAt(-1, -1).TraderHere = TraderFactory.GetTraderByName("Farmer Ted");
            newWorld.AddLocation(-2, -1, "Farm Fields",
                "Infestation of rats.",
                "FarmFields.PNG");
            newWorld.LocationAt(-2, -1).AddMonster(2, 100);
            newWorld.AddLocation(-1, 0, "Trading Shop",
                "You can buy or sell items here.",
                "Trader.PNG");
            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderByName("Susan");
            
            return newWorld;
        }
    }
}
