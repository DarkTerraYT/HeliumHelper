using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level20 : WendellLevel
    {
        public override int Level => 20;

        public override string Description => "Treaty now befriends all bloons BFB and under. Wendell befriends moabs 20% faster (25s -> 20s), and auto send is now every .1s.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAbility(1).name = "AbilityModel_Treaty_Lvl20";
            towerModel.GetWeapon(1).rate = 20;
        }
    }
}
