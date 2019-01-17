using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class Monster :LivingEntity
    {
        
        public string ImageName { get; set; }   
        public int MaximumDamage { get; set; }
        public int MinimumDamage { get; set; }
        public int RewardEXPPoint { get; set; } 

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
