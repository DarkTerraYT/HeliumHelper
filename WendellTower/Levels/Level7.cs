using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;

namespace HeliumHelper.WendellTower.Levels
{
    public class Level7 : WendellLevel
    {
        public override int Level => 7;

        public override string Description => "Wendell befriends bloons 43% faster (7s -> 4s), and can befriend black/white/purple bloons. All bloons can now hit camo.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            IncreaseDamage(1, towerModel);
            towerModel.GetWeapon().rate = 4;
        }
    }
}
