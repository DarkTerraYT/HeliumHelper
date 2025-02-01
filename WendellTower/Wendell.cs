using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using HeliumHelper.UI;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using MelonLoader;
using System;
using System.Linq;

namespace HeliumHelper.WendellTower
{
    public class Wendell : ModHero
    {

        public override string BaseTower => TowerType.DartMonkey;

        public override string Icon => Portrait;

        public override int Cost => 500;

        public override string Description => "Wendell befriends the bloons around them and gets them to help the monkeys!";

        public override float XpRatio => 1;

        public override string Title => "The Helium Helper";

        public override string Level1Description => "Wendell befriends all bloons green and under every 8 seconds. He can send these bloons to fight later.";

        public override string Square => "Wendell-Square";

        public override string BackgroundStyle => TowerType.AdmiralBrickell;

        public override string GlowStyle => TowerType.Quincy;

        public override string NameStyle => TowerType.ObynGreenfoot;

        public override string Button => "Icon";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var wpn = towerModel.GetWeapon();
            wpn.rate = 10;
            wpn.SetEject(new UnityEngine.Vector3(0, 0, 0));

            var proj = towerModel.GetWeapon().projectile;

            proj.GetDamageModel().damage = 99;
            proj.GetDamageModel().maxDamage = 99;
            proj.id = "Wendell_Befriend";

            proj.display = new("");
            proj.RemoveBehavior<TravelStraitModel>();

            proj.AddBehavior(new AgeModel("AgeModel", 0.1f, 1, false, null));

            proj.pierce = 9999999;

            LinkProjectileRadiusToTowerRangeModel linkProjectileRadiusToTowerRangeModel = new("LinkProjectileRadiusToTowerRangeModel_", proj, towerModel.range, 0, 0);
            towerModel.AddBehavior(linkProjectileRadiusToTowerRangeModel);

            towerModel.range = 30;
            towerModel.GetAttackModel().range = 30;
            towerModel.GetAttackModel().AddFilter(new FilterMoabModel("FilterMoabModel", true));
        }

        public void OnSelect(Tower wendell, TowerSelectionMenu tsm)
        {
            WendellBloonsButton.Create(tsm);
        }

        public void OnBefriend(Tower wendell, Bloon bloon)
        {
            foreach (var level in GetContent<WendellLevel>().FindAll(level => level.Level <= wendell.GetTowerBehavior<Hero>().GetHeroLevel()))
            {
                level.OnBefriend(wendell, bloon, wendell.GetTowerBehavior<Hero>().GetHeroLevel());
            }
            HeliumHelperMod.BefriendBloon(bloon.bloonModel.id, true);
        }
    }

    public class WendellLvl1 : ModTowerCustomDisplay<Wendell>
    {
        public override string AssetBundleName => "wendell";

        public override string PrefabName => Name;

        public override bool UseForTower(params int[] tiers) => tiers.Sum() < 7;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            string[] redOutlines = ["Gloves", "Hat"];
            string[] brownOutlines = ["Monkey"];

            foreach(var rend in node.GetMeshRenderers())
            {
                rend.ApplyOutlineShader();
                if (redOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(150, 20, 20, 255));
                }
                else if(brownOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(120, 59, 26, 255));
                }
            }
        }
    }
    public class WendellLvl3 : ModTowerCustomDisplay<Wendell>
    {
        public override string AssetBundleName => "wendell";

        public override string PrefabName => Name;

        public override bool UseForTower(params int[] tiers) => tiers.Sum() >= 3 && tiers.Sum() < 7;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            string[] redOutlines = ["Gloves", "Hat"];
            string[] brownOutlines = ["Monkey"];

            foreach (var rend in node.GetMeshRenderers())
            {
                rend.ApplyOutlineShader();
                if (redOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(150, 20, 20, 255));
                }
                else if (brownOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(120, 59, 26, 255));
                }
            }
        }
    }
    public class WendellLvl7 : ModTowerCustomDisplay<Wendell>
    {
        public override string AssetBundleName => "wendell";

        public override string PrefabName => Name;

        public override bool UseForTower(params int[] tiers) => tiers.Sum() >= 7 && tiers.Sum() < 10;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            string[] redOutlines = ["Glasses"];
            string[] brownOutlines = ["Monkey"];
            string[] greenOutlines = ["Gloves", "Hat"];

            foreach (var rend in node.GetMeshRenderers())
            {
                rend.ApplyOutlineShader();
                if (redOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(150, 20, 20, 255));
                }
                else if (brownOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(120, 59, 26, 255));
                }
                else if (greenOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(58, 125, 29, 255));
                }
            }
        }
    }
    public class WendellLvl10 : ModTowerCustomDisplay<Wendell>
    {
        public override string AssetBundleName => "wendell";

        public override string PrefabName => Name;

        public override bool UseForTower(params int[] tiers) => tiers.Sum() >= 10 && tiers.Sum() < 20;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            string[] redOutlines = ["Glasses"];
            string[] brownOutlines = ["Monkey"];
            string[] greenOutlines = ["Gloves", "Hat", "Monkey.001"];

            foreach (var rend in node.GetMeshRenderers())
            {
                rend.ApplyOutlineShader();
                if (redOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(150, 20, 20, 255));
                }
                else if (brownOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(120, 59, 26, 255));
                }
                else if (greenOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(58, 125, 29, 255));
                }
            }
        }
    }
    public class WendellLvl20 : ModTowerCustomDisplay<Wendell>
    {
        public override string AssetBundleName => "wendell";

        public override string PrefabName => Name;

        public override bool UseForTower(params int[] tiers) => tiers.Sum() == 20;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            string[] redOutlines = ["Glasses"];
            string[] brownOutlines = ["Monkey"];
            string[] pinkOutlines = ["Gloves", "Hat", "Monkey.001"];

            foreach (var rend in node.GetMeshRenderers())
            {
                rend.ApplyOutlineShader();
                if (redOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(150, 20, 20, 255));
                }
                else if (brownOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(120, 59, 26, 255));
                }
                else if(pinkOutlines.Contains(rend.name))
                {
                    rend.SetOutlineColor(new UnityEngine.Color32(143, 37, 102, 255));
                }
            }
        }
    }
}
