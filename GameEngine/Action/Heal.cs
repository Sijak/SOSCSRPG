using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Action
{
    public class Heal : IAction
    {
        public readonly GameItem _item;
        public readonly int _hitPointsToHeal;

        public event EventHandler<string> OnActionPerformed;

        public Heal(GameItem item, int hitPointToHeal)
        {
            if(item.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{item.Name} is not consumable item.");
            }
            _item = item;
            _hitPointsToHeal = hitPointToHeal;
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player)? "You": $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";
            ReportResult($"{actorName} has healed {targetName}.");
            target.Heal(_hitPointsToHeal);            
        }

        private void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
