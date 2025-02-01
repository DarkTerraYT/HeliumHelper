using Il2CppAssets.Scripts.Models.Towers;
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

        public override string Description => "Chance of keeping bloons increased to 1/10.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
        }
    }
}
