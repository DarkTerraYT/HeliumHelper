using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level6 : WendellLevel
    {
        public override int Level => 6;

        public override string Description => "Wendell can now befriend pink bloons.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel);
        }
    }
}
