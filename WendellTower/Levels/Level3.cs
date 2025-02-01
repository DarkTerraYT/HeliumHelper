using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level3 : WendellLevel
    {
        public override int Level => 3;

        public override string Description => "Ability, Referral: Wendell gets 2x bloons for the next 10 seconds";

        public override string AbilityDescription => "Referral: Wendell gets 2x bloons for the next 10 seconds";

        AbilityModel abilityModel()
        {
            TurboModel turboModel = new("Turbo_Model", 10, 1, null, 0, 0, false);

            AbilityModel ability = Game.instance.model.GetTowerFromId("MortarMonkey-040").GetAbility().Duplicate();
            ability.behaviors[0] = turboModel;

            ability.cooldown = 45;

            ability.name = "referral";
            ability.description = "Referral: Wendell gets 2x bloons for the next 10 seconds";
            ability.displayName = "Referral";

            ability.icon = GetSpriteReference("ReferralAA");
            ability.addedViaUpgrade = Id;


            return ability;
        }

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.AddBehavior(abilityModel());
        }

        public override void OnBefriend(Tower wendell, Bloon bloon, int currentLevel)
        {
            if (wendell.GetTowerBehaviors<Ability>().ToList().Find(aa => aa.abilityModel.displayName == abilityModel().displayName).IsActive)
            {
                HeliumHelperMod.WendellBloons.TryAdd(bloon.bloonModel.id, 0);
                HeliumHelperMod.WendellBloons[bloon.bloonModel.id] += 1;
            }
        }
    }
}
