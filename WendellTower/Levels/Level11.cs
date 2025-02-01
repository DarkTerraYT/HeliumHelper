using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    internal class Level11 : WendellLevel
    {
        public override int Level => 11;

        public override string Description => "Range increased. Bloons can now hit all bloon types.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.IncreaseRange(10);
        }
    }
}
