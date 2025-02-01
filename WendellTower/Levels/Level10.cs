using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level10 : WendellLevel
    {
        public override int Level => 10;

        public override string AbilityDescription => "Treaty: Every bloon on the map under a zebra/lead bloon instantly gets befriended.";

        public override string Description => $"Ability, {AbilityDescription}";


        AbilityModel abilityModel()
        {
            AbilityModel ability = Game.instance.model.GetTowerFromId("MortarMonkey-040").GetAbility().Duplicate();
            ability.behaviors = null;

            ability.cooldown = 75;

            ability.name = "AbilityModel_Treaty_Lvl10";
            ability.description = AbilityDescription;
            ability.displayName = "Treaty";

            ability.icon = GetSpriteReference("TreatyAA");
            ability.addedViaUpgrade = Id;


            return ability;
        }

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.AddBehavior(abilityModel());
        }
    }
}
