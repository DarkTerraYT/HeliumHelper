using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level17 : WendellLevel
    {
        public override int Level => 17;

        public override string Description => "Wendell befriends MOAB-Class bloons 16.7% times faster (30s -> 25s)";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetWeapon(1).rate = 25;
        }
    }
}
