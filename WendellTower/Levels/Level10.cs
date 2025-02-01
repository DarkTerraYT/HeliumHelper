using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level10 : WendellLevel
    {
        public override int Level => 10;

        public override string AbilityDescription => "Treaty: Every bloon on the map under a zebra/lead bloon instantly gets befriended.";

        public override string Description => $"Ability, {AbilityDescription}";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.AddBehavior(new AbilityModel("AbilityModel_Treaty_Lvl10", "Treaty", AbilityDescription, 0, 0, GetSpriteReference("TreatyAA"), 75, new([]), false, false, Id, 1, 0, 999999, false, false));
        }
    }
}
