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
    public class Level15 : WendellLevel
    {
        public override int Level => 15;

        public override string Description => "When befriending a moab bloon, have a 1/250 chance to get a money moab, which increases the value of every bloon hit by $20. Regular money bloon chance also increased to this.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {

        }

        public override void OnBefriend(Tower wendell, Bloon bloon, int currentLevel)
        {

            if (!bloon.bloonModel.IsMoabBloon())
            {
                return;
            }

            switch (currentLevel)
            {
                case < 19:
                    Random rand = new Random();
                    int randNum = rand.Next(251);

                    if (randNum == 250)
                    {
                        HeliumHelperMod.BefriendBloon(BloonID<MoneyMoab>());
                    }
                    break;
                case >= 19:

                    rand = new Random();
                    randNum = rand.Next(101);

                    if (randNum == 100)
                    {
                        HeliumHelperMod.BefriendBloon(BloonID<MoneyMoab>());
                    }
                    break;
            }
        }
    }
}
