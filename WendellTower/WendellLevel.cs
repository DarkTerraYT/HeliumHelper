using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace HeliumHelper.WendellTower
{
    public abstract class WendellLevel : ModHeroLevel<Wendell>
    {
        protected void IncreaseDamage(float dmg, TowerModel wendell, bool moab = false)
        {
            if (!moab)
            {
                wendell.GetWeapon().projectile.GetDamageModel().damage += dmg;
                wendell.GetWeapon().projectile.GetDamageModel().maxDamage += dmg;
            }
            else
            {
                wendell.GetWeapon(1).projectile.GetDamageModel().damage += dmg;
                wendell.GetWeapon(1).projectile.GetDamageModel().maxDamage += dmg;
            }
        }

        public virtual void OnBefriend(Tower wendell, Bloon bloon, int currentLevel)
        {

        }
    }
}
