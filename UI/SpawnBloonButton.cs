using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using HeliumHelper.WendellTower;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using MelonLoader;
using System;
using System.Linq;

namespace HeliumHelper.UI
{
    [RegisterTypeInIl2Cpp(false)]
    public class SpawnBloonButton : ModHelperComponent
    {
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.Expire))]
        static class Projectile_Expire
        {
            [HarmonyPostfix]
            public static void Postfix(Projectile __instance)
            {
                if(__instance.projectileModel.id.Contains("BloonSpawn"))
                {
                    Random rand = new Random();
                    float num = (float)rand.NextDouble();

                    if (num <= HeliumHelperMod.KeepBloonChance)
                    {
                        var bloonId = __instance.projectileModel.id.Split('_')[1];

                        HeliumHelperMod.WendellBloons.TryAdd(bloonId, 0);
                        HeliumHelperMod.WendellBloons[bloonId] += 1;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.Initialise))]
        static class Projectile_Initialise
        {
            [HarmonyPostfix]
            public static void Postfix(Projectile __instance)
            {
                if (__instance.projectileModel.id.Contains("BloonSpawn_Child"))
                {
                    __instance.GetProjectileBehavior<TrackTargetOrOrbitTower>().OrbitTower();
                }
            }
        }


        public ModHelperText countText;
        public SpriteReference icon;
        public ModHelperImage iconImage;
        public ModHelperButton button;
        public ModHelperText nameText;

        public const float accelerationRate = 0.1f;
        public const float turnRate = 0.25f;

        public string displayName;

        public string bloonId;

        public void Remove()
        {
            HeliumHelperMod.WendellBloons.Remove(bloonId);
            gameObject.Destroy();
        }

        void onClick()
        {

            SpawnProjectile(bloonId);

            HeliumHelperMod.WendellBloons[bloonId] -= 1;

            if(HeliumHelperMod.WendellBloons[bloonId] < 1)
            {
                Remove();
                return;
            }

            countText.SetText(HeliumHelperMod.WendellBloons[bloonId].ToString());
        }

        public void Register()
        {
            button.Button.SetOnClick(onClick);

            icon = Game.instance.model.bloonsByName[bloonId].icon;

            iconImage = button.AddImage(new("Icon", 300), icon.AssetGUID);

            countText = button.AddText(new("Count", 0, -125f, 325, 80), HeliumHelperMod.WendellBloons[bloonId].ToString());
            countText.Text.enableAutoSizing = true;
            countText.Text.fontSizeMax = 75;
        }

        public SpawnBloonButton(IntPtr ptr) : base(ptr) { }

        static void AddProjectileBehaviorsForBloonType(string bloon, ProjectileModel model)
        {
            model.SetHitCamo(Game.instance.model.bloonsByName[bloon].isCamo);

            if (bloon.Contains(ModContent.BloonID<MoneyBloon>())) 
            {
                model.AddBehavior<IncreaseBloonWorthModel>(new("IncreaseBloonWorthModel_", "MoneyBloon", 1, 1, null, 1, "", new("MoneyBloon", 1, 1, "")));
            }
            if (bloon.Contains(ModContent.BloonID<MoneyMoab>()))
            {
                model.AddBehavior<IncreaseBloonWorthModel>(new("IncreaseBloonWorthModel_", "MoneyMoab", 1, 10, null, 1, "", new("MoneyMoab", 1, 10, "")));
            }
        }

        static ProjectileModel BloonProjectile(string bloon, string projIdSuffix = "")
        {
            var model = Game.instance.model.GetTower("DartMonkey").GetWeapon().projectile.Duplicate();
            var bloonModel = Game.instance.model.bloonsByName[bloon];

            model.pierce = bloonModel.maxHealth;
            model.GetDamageModel().damage = MathF.Ceiling(bloonModel.leakDamage / 10);

            model.id = "BloonSpawn_" + bloon + "_" + projIdSuffix;

            AddProjectileBehaviorsForBloonType(bloon, model);

            model.display = bloonModel.display;

            model.RemoveBehavior<TravelStraitModel>();
            AgeModel ageModel = new("AgeModel_", 999999, 0, false, null);
            model.AddBehavior(ageModel);

            TrackTargetOrOrbitTowerModel trackTargetOrOrbitTowerModel = new("TrackTargetOrOrbitTowerModel_", 0, 0, bloonModel.speed / 10, bloonModel.speed / 10, bloonModel.speed / 10 * accelerationRate, 360 * turnRate, 360, bloonModel.speed / 15, 10 + bloonModel.radius * 1.25f, 0.01f, 1, null);
            model.AddBehavior(trackTargetOrOrbitTowerModel);

            model.radius = Game.instance.model.bloonsByName[bloon].radius;

            model.ignoreBlockers = true;

            return model;
        }

        static void SpawnProjectile(string bloon)
        {
            var model = BloonProjectile(bloon);

            var tower = InGame.instance.GetTowers().Find(twr => twr.towerModel.baseId == ModContent.TowerID<Wendell>());

            if(tower.towerModel.tier >= 11)
            {
                model.SetHitCamo(true);
                model.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            }

            var proj = InGame.instance.GetMainFactory().CreateEntityWithBehavior<Projectile, ProjectileModel>(model);

            var point = tower.GetTowerToSim().simPosition;

            proj.Rotation = 180;

            proj.Position.X = point.x;
            proj.Position.Y = point.y;
            proj.Position.Z = point.z + 5;
            proj.emittedFrom = point;
            proj.emittedBy = tower;

            proj.GetProjectileBehavior<TrackTargetOrOrbitTower>().OrbitTower();
        }

        public class BloonSpawnDisplay(string bloon) : ModDisplay
        {
            public override string BaseDisplay => GetBloonDisplay(bloon);
        }

        public static SpawnBloonButton Create(string bloonId)
        {
            string guid = "";

            float danger = Game.instance.model.bloonsByName[bloonId].danger;

            switch (danger)
            {
                case <=1:
                    guid = VanillaSprites.GreyInsertPanel;
                    break;
                case < 6:
                    guid = VanillaSprites.MainBGPanelBronze;
                    break;
                case < 10:
                    guid = VanillaSprites.MainBGPanelSilver;
                    break;
                case < 14:
                    guid = VanillaSprites.MainBGPanelGold;
                    break;
                case >=14:
                    guid = VanillaSprites.MainBgPanelHematite;
                    break;
            }

            var btn = ModHelperButton.Create(new("Spawn_" + bloonId, 500), guid, new Action(() => { }));

            SpawnBloonButton spawnBloonButton = btn.AddComponent<SpawnBloonButton>();

            spawnBloonButton.button = btn;
            spawnBloonButton.bloonId = bloonId;

            spawnBloonButton.Register();
            return spawnBloonButton;
        }
    }
}
