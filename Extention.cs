using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper
{
    public static class Extention
    {
        public static Tower GetTower(this Projectile projectile)
        {
            return InGame.instance.GetTowers().Find(twr => twr.Id == projectile.EmittedByTowerId);
        }

        public static int GetRBE(this Bloon bloon)
        {
            var bloonModel = bloon.rootModel.Cast<BloonModel>();

            return GetRBE(bloonModel);
        }

        public static int GetRBE(this BloonModel bloonModel)
        {

            if (bloonModel.childBloonModels.Count < 1)
            {
                return 1;
            }

            int rbe = 0;

            foreach (var child in bloonModel.childBloonModels)
            {
                rbe += child.GetRBE();
            }

            return rbe;
        }
    }
}
