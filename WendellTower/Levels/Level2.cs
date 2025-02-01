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
    public class Level2 : WendellLevel
    {
        public override int Level => 2;

        public override string Description => "Wendell can befriend camo bloons. Camo bloons can pop other camo bloons.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(mod => mod.isActive = false);
        }
    }
}
