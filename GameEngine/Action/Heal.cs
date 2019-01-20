using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Action
{
    public class Heal : BaseAction, IAction
    {
        
        private readonly int _hitPointsToHeal;



        public Heal(GameItem iteminUse, int hitPointToHeal) : base(iteminUse)
        {
            if(iteminUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{iteminUse.Name} is not consumable item.");
            }
            
            _hitPointsToHeal = hitPointToHeal;
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player)? "You": $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";
            ReportResult($"{actorName} has healed {targetName}.");
            target.Heal(_hitPointsToHeal);            
        }

       
    }
}
