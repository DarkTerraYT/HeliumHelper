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
    public class Level16 : WendellLevel
    {
        public override int Level => 16;

        public override string Description => "Wendell can befriend BFBs";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel, true);
        }
    }
}
