﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class MonsterEncounter
    {
        public int MonsterID { get; }          
        public int ChanceOfEncounter { get; set; }

        public MonsterEncounter(int monsterID, int chanceOfEncounter)
        {
            MonsterID = monsterID;
            ChanceOfEncounter = chanceOfEncounter;
        }

    }
}
