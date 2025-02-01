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
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Models.Towers;
using HeliumHelper.WendellTower.Levels;
using Il2CppAssets.Scripts.Models.Bloons;
using System.Linq;
using UnityEngine;

[assembly: MelonInfo(typeof(HeliumHelper.HeliumHelperMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace HeliumHelper;

public class HeliumHelperMod : BloonsTD6Mod
{
    public static List<string> WhitelistedBloons = [];

    public static bool AutoSend = true;

    public static float AutoSendSpeed = 2;

    #region Common Methods and Patches
    public static void BefriendBloon(string id, bool updateMenu = false)
    {
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
                if (__instance.bloonModel.layerNumber <= projectile.projectileModel.GetDamageModel().damage && !__instance.bloonModel.isBoss)
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
    public override void OnLateUpdate()
    {
        if (InGame.instance != null && InGame.instance.bridge != null && InGame.instance.GetTowers().Any(twr => twr.towerModel.baseId == ModContent.TowerID<Wendell>()))
        {
            if (WendellBloonsMenu.autoSendEnabled && WendellBloonsMenu.nextAutoSendTime <= Time.time && WendellBloons.Count > 0)
            {
                WendellBloonsMenu.nextAutoSendTime = Time.time + AutoSendSpeed;

                List<BloonModel> bloons = [];

                foreach (var id in WendellBloons.Keys)
                {
                    bloons.Add(Game.instance.model.bloonsByName[id]);
                }

                string bloon = bloons.OrderBy(bm => bm.danger).ToList()[bloons.Count - 1].id;

                SpawnBloonButton.SpawnProjectile(bloon);

                WendellBloons[bloon] -= 1;
                if (WendellBloons[bloon] <= 0)
                {
                    WendellBloons.Remove(bloon);
                }
            }
        }
    }

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
        if (upgradeName == ModContent.UpgradeID<Level16>())
        {
            WhitelistedBloons.Add("Ddt");
        }
        else if (upgradeName == ModContent.UpgradeID<Level5>())
        {
            AutoSend = true;
            AutoSendSpeed = 2;
        }
        else if (upgradeName == ModContent.UpgradeID<Level13>())
        {
            AutoSendSpeed = 1;
        }
        else if (upgradeName == ModContent.UpgradeID<Level17>())
        {
            AutoSendSpeed = .5f;
        }
        else if (upgradeName == ModContent.UpgradeID<Level20>())
        {
            AutoSendSpeed = .1f;
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
    public const string AutoSendID = "WendellAutoSendBloons";
    public const string AutoSendSpeedID = "WendellAutoSendBloonsSpeed";

    [HarmonyPatch(typeof(Map), nameof(Map.GetSaveData))]
    static class Map_GetSaveData
    {
        [HarmonyPostfix]
        public static void Postfix(MapSaveDataModel mapData)
        {
            var json = JsonConvert.SerializeObject(WendellBloons);
            if (!mapData.metaData.TryAdd(WendellBloonsID, json))
            {
                mapData.metaData[WendellBloonsID] = json;
            }
            json = JsonConvert.SerializeObject(AutoSend);
            if (!mapData.metaData.TryAdd(AutoSendID, json))
            {
                mapData.metaData[AutoSendID] = json;
            }
            json = JsonConvert.SerializeObject(AutoSendSpeed);
            if (!mapData.metaData.TryAdd(AutoSendSpeedID, json))
            {
                mapData.metaData[AutoSendSpeedID] = json;
            }
        }
    }

    [HarmonyPatch(typeof(Map), nameof(Map.SetSaveData))]
    static class Map_SetSaveData
    {
        [HarmonyPostfix]
        public static void Postfix(MapSaveDataModel mapData)
        {
            if (mapData.metaData.TryGetValue(WendellBloonsID, out var data))
            {
                WendellBloons = JsonConvert.DeserializeObject<Dictionary<string, int>>(data)!;
            }
            if (mapData.metaData.TryGetValue(AutoSendID, out data))
            {
                AutoSend = JsonConvert.DeserializeObject<bool>(data)!;
            }
            if (mapData.metaData.TryGetValue(AutoSendSpeedID, out data))
            {
                AutoSendSpeed = JsonConvert.DeserializeObject<float>(data)!;
            }
        }
    }
    #endregion
}