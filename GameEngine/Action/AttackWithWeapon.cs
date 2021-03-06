﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Action
{
    public class AttackWithWeapon : BaseAction,IAction
    {
        
        private readonly int _minimumDamage;
        private readonly int _maximumDamage;

        

        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) :base(itemInUse)
        {
            if(itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon.");
            }
            if(minimumDamage<0)
            {
                throw new ArgumentException("Minimum Damage must be 0 or larger.");
            }
            if(minimumDamage>maximumDamage)
            {
                throw new ArgumentException("Maximum Damage must be >= Minimum Damage.");
            }
            
            _maximumDamage = maximumDamage;
            _minimumDamage = minimumDamage;

        }
        public void Execute (LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage,_maximumDamage);

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {actor.Name.ToLower()}";

            if (damage == 0)
            {
                ReportResult($"{actorName} missed {targetName}.");
            }
            else
            {
                ReportResult($"{actorName} hit the {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }

        }

        
    }
}
