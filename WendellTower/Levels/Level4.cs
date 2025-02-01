using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level4 : WendellLevel
    {
        public override int Level => 4;

        public override string Description => "Wendell can now befriend yellow bloons. He also tames them 1 second faster.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel);

            towerModel.GetWeapon().rate -= 1;
        }
    }
}
