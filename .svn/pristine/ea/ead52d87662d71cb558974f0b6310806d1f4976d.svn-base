using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class Monster :LivingEntity
    {
        
        public string ImageName { get; }   
        public int MaximumDamage { get; }
        public int MinimumDamage { get; }
        public int RewardEXPPoint { get; } 

        public Monster(string name, string imageName, int maximumHitPoint, 
            int hitPoint,int maximumDamage, int minimumDamage, int rewardEXPPoint, int rewardGold)
            :base (name, hitPoint, maximumHitPoint,rewardGold)
        {
            
            ImageName = string.Format($"/GameEngine;component/Images/Monsters/{imageName}");
            
            MaximumDamage = maximumDamage;
            MinimumDamage = minimumDamage;
            RewardEXPPoint = rewardEXPPoint;
            
            

        }
        
    }  
    
}
