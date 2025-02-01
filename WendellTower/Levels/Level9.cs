using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level9 : WendellLevel
    {
        public override int Level => 9;

        public override string Description => "Wendell can now befriend up to a zebra/lead bloon.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel);
        }
    }
}
