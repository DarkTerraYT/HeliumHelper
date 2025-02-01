using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliumHelper.WendellTower
{
    public class WendellParagon : ModParagonUpgrade<Wendell>
    {
        public override int Cost => 1;

        public override void ApplyUpgrade(TowerModel towerModel)
        {

        }

        public void OnBefriend(Tower wendell)
        {

        }
    }
}
