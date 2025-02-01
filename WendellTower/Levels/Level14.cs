using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level14 : WendellLevel
    {
        public override int Level => 14;

        public override string Description => "Wendell can befriend Ceramics. Also can befriend MOABs every 30 seconds.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attack = towerModel.GetAttackModel().Duplicate();
            var weapon = attack.weapons[0];
            weapon.rate = 30;
            attack.GetDescendant<FilterMoabModel>().flip = false;
            attack.name = "Wendell_Moabs";

            weapon.name = "Wendell_Moabs";

            towerModel.AddBehavior(attack);
        }
    }
}
