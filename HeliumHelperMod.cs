using MelonLoader;
using BTD_Mod_Helper;
using HeliumHelper;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using Newtonsoft.Json;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using BTD_Mod_Helper.Api;
using HeliumHelper.WendellTower;
using Il2CppAssets.Scripts.Unity;
using HeliumHelper.UI;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Track;
using MelonLoader.Utils;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using HeliumHelper.WendellTower.Levels;

[assembly: MelonInfo(typeof(HeliumHelper.HeliumHelperMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace HeliumHelper;

public class HeliumHelperMod : BloonsTD6Mod
{
    public static float KeepBloonChance = 0.00f;

    public static List<string> WhitelistedBloons = [];

    #region Common Methods and Patches
    public static void BefriendBloon(string id, bool updateMenu = false)
    {
        KeepBloonChance = 0;

        WendellBloons.TryAdd(id, 0);
        WendellBloons[id] += 1;

        if (WendellBloonsMenu.instance != null && updateMenu)
        {
            if (WendellBloonsMenu.instance.updateToken != null)
            {
                MelonCoroutines.Stop(WendellBloonsMenu.instance.updateToken);
            }

            WendellBloonsMenu.instance.updateToken = MelonCoroutines.Start(WendellBloonsMenu.instance.UpdateButtons());
        }
    }

    [HarmonyPatch(typeof(TowerSelectionMenu), nameof(TowerSelectionMenu.SelectTower))]
    static class TowerSelectionMenu_Open
    {
        [HarmonyPostfix]
        static void Postfix(TowerSelectionMenu __instance, TowerToSimulation tower)
        {
            if (tower.tower.towerModel.baseId == ModContent.TowerID<Wendell>())
            {
                ModContent.GetInstance<Wendell>().OnSelect(tower.tower, __instance);
            }
            else
            {
                if (WendellBloonsButton.instance != null)
                {
                    WendellBloonsButton.instance.Close();
                }
            }
        }
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
    static class Bloon_Damage
    {
        [HarmonyPrefix]
        static bool Prefix(Bloon __instance, ref float totalAmount, Projectile projectile, Tower tower, ref bool createEffect, ref bool distributeToChildren)
        {
            if (tower == null || projectile == null)
            {
                return true;
            }

            if (projectile.projectileModel.id == "Wendell_Befriend")
            {
                if (__instance.bloonModel.layerNumber <= projectile.projectileModel.GetDamageModel().damage)
                {
                    ModContent.GetInstance<Wendell>().OnBefriend(tower, __instance);
                    totalAmount = 1999999999;
                    createEffect = false;
                    distributeToChildren = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Ability), nameof(Ability.Activate))]
    static class Ability_Cast
    {
        [HarmonyPostfix]
        public static void Postfix(Ability __instance)
        {
            if(__instance.abilityModel.name == "AbilityModel_Treaty_Lvl10")
            {
                foreach(var bloon in InGame.instance.GetBloons())
                {
                    if(bloon.bloonModel.layerNumber <= 6)
                    {
                        ModContent.GetInstance<Wendell>().OnBefriend(__instance.tower, bloon);
                        bloon.Damage(1999999999, null, true, true, false);
                    }
                }
            }
            else if (__instance.abilityModel.name == "AbilityModel_Treaty_Lvl20")
            {
                foreach (var bloon in InGame.instance.GetBloons())
                {
                    if (bloon.bloonModel.layerNumber <= 11)
                    {
                        ModContent.GetInstance<Wendell>().OnBefriend(__instance.tower, bloon);
                        bloon.Damage(1999999999, null, true, true, false);
                    }
                }
            }
        }
    }
    #endregion
    #region Hooks
    public override void OnWeaponFire(Weapon weapon)
    {
        var tower = weapon.attack.tower;

        var proj = weapon.weaponModel.projectile;

        if (tower.towerModel.baseId == ModContent.TowerID<Wendell>())
        {
            if (proj.id == "Wendell_Befriend")
            {
                tower.GetUnityDisplayNode().animationComponent.SetTrigger("Befriend");
            }
        }
    }

    public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
    {
        if(upgradeName == ModContent.UpgradeID<Level16>())
        {
            WhitelistedBloons.Add("Ddt");
        }
    }

    public override void OnMatchEnd()
    {
        WendellBloons.Clear();
        WhitelistedBloons.Clear();
    }

    public override void OnRestart()
    {
        WendellBloons.Clear();
        WhitelistedBloons.Clear();
    }
    #endregion

    #region Saving
    public const string WendellBloonsID = "WendellBloons";
    public static Dictionary<string, int> WendellBloons = [];

    [HarmonyPatch(typeof(Map), nameof(Map.GetSaveData))]
    static class Map_GetSaveData
    {
        [HarmonyPostfix]
        public static void Postfix(MapSaveDataModel mapData)
        {
            if (mapData.metaData.TryGetValue(WendellBloonsID, out var data))
            {
                WendellBloons = JsonConvert.DeserializeObject<Dictionary<string, int>>(data)!;
            }
        }
    }

    [HarmonyPatch(typeof(Map), nameof(Map.SetSaveData))]
    static class Map_SetSaveData
    {
        [HarmonyPostfix]
        public static void Postfix(MapSaveDataModel mapData)
        {
            var json = JsonConvert.SerializeObject(WendellBloons);
            mapData.metaData[WendellBloonsID] = json;

        }
    }
    #endregion
}