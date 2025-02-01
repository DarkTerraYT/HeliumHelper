using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level18 : WendellLevel
    {
        public override int Level => 18;

        public override string Description => "Wendell can now befriend ZOMGs.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel, true);
        }
    }
}
