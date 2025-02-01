using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level19 : WendellLevel
    {
        public override int Level => 19;

        public override string Description => "Every 2.5 minutes, wendell can befriend a BAD.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var atk = towerModel.GetAttackModel(1).Duplicate();
            atk.RemoveFilter<FilterMoabModel>();
            atk.AddFilter(new FilterWithTagModel("FilterWithTagModel", "Bad", false));
            var wpn = atk.weapons[0];

            atk.name = "Wendell_Bads";

            wpn.projectile.GetDamageModel().damage = 13;
            wpn.projectile.GetDamageModel().maxDamage = 13;
            wpn.rate = 150;
            wpn.name = "Wendell_Bads";

            towerModel.AddBehavior(atk);
        }
    }
}
