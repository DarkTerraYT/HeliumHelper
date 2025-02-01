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
    public class Level8 : WendellLevel
    {
        public override int Level => 8;

        public override string Description => "Wendell has a 0.1% chance to get a money bloon upon befriending a non-moab class bloon. This bloon increases pop value of 15 bloons by 1.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {

        }

        public override void OnBefriend(Tower wendell, Bloon bloon, int currentLevel)
        {
            if (bloon.bloonModel.IsMoabBloon())
            {
                return;
            }

            switch (currentLevel)
            {
                case <15:
                    Random rand = new Random();
                    int randNum = rand.Next(1001);

                    if (randNum == 1000)
                    {
                        HeliumHelperMod.BefriendBloon(BloonID<MoneyBloon>());
                    }
                    break;
                case >= 15:

                    rand = new Random();
                    randNum = rand.Next(251);

                    if (randNum == 250)
                    {
                        HeliumHelperMod.BefriendBloon(BloonID<MoneyBloon>());
                    }
                    break;
            }
        }
    }
}
