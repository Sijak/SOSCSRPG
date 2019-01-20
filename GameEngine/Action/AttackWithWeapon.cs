using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Action
{
    public class AttackWithWeapon : IAction
    {
        private readonly GameItem _weapon;
        private readonly int _minimumDamage;
        private readonly int _maximumDamage;

        public event EventHandler<String> OnActionPerformed;

        public AttackWithWeapon(GameItem weapon, int minimumDamage, int maximumDamage)
        {
            if(weapon.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{weapon.Name} is not a weapon.");
            }
            if(minimumDamage<0)
            {
                throw new ArgumentException("Minimum Damage must be 0 or larger.");
            }
            if(minimumDamage>maximumDamage)
            {
                throw new ArgumentException("Maximum Damage must be >= Minimum Damage.");
            }
            _weapon = weapon;
            _maximumDamage = maximumDamage;
            _minimumDamage = minimumDamage;

        }
        public void Execute (LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage,_maximumDamage);
            if (damage == 0)
            {
                ReportResult($"You missed {target.Name}.");
            }
            else
            {
                ReportResult($"You hit the {target.Name} for {damage} points of damage.");
                target.TakeDamage(damage);
            }

        }

        private void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
