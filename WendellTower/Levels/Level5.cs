using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level5 : WendellLevel
    {
        public override int Level => 5;

        public override string Description => "Wendell sends out your best bloon every 2 seconds (toggleable in bloons menu). Range is also increased.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.IncreaseRange(5);
        }
    }
}
