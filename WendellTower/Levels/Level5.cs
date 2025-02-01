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

        public override string Description => "Befriended bloons have a 1/20 chance of returning after they finish fighting if they were not popped. Range is also increased.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.IncreaseRange(5);
        }

        public override void OnBefriend(Tower wendell, Bloon bloon, int currentLevel)
        {
            switch (currentLevel)
            {
                case <13:
                    HeliumHelperMod.KeepBloonChance = 1 / 20f;
                    break;
                case < 17:
                    HeliumHelperMod.KeepBloonChance = 1 / 10f;
                    break;
                case >= 17:
                    HeliumHelperMod.KeepBloonChance = 1 / 5f;
                    break;
            }
        }
    }
}
