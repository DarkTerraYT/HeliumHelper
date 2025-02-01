using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level13 : WendellLevel
    {
        public override int Level => 13;

        public override string Description => "Auto send speed now 1s. Referral lasts twice as long.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAbility().GetBehavior<TurboModel>().lifespan *= 2;
        }
    }
}
