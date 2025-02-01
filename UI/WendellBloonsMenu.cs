using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Track;
using BTD_Mod_Helper.Api;
using HeliumHelper.WendellTower;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using System.Threading.Tasks;
using System.Collections;
using Il2CppAssets.Scripts.Unity.Achievements.List;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;

namespace HeliumHelper.UI
{
    [RegisterTypeInIl2Cpp(false)]
    public class WendellBloonsMenu : MonoBehaviour
    {
        public static WendellBloonsMenu instance;

        public GridLayoutGroup layoutGroup;

        public ModHelperPanel mainPanel;

        internal object updateToken;

        public ModHelperScrollPanel scrollPanel;

        public Dictionary<string, int> WendellBloonsOld = [];
        TowerSelectionMenu tsm;

        MatchLocalPosition matchLocalPosition;

        bool readyForUpdate = false;

        [HarmonyPatch(typeof(TowerSelectionMenu), nameof(TowerSelectionMenu.DeselectTower))]
        static class TSM_Deselect
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (instance != null)
                {
                    instance.gameObject.SetActive(false);
                }
            }
        }

        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
                instance = null;

                if(tsm.selectedTower != null)
                {
                    if (tsm.selectedTower.tower.towerModel.baseId == ModContent.TowerID<Wendell>())
                    {
                        WendellBloonsButton.Create(tsm);
                    }
                }
            }
        }

        [HideFromIl2Cpp]
        public IEnumerator UpdateButtons()
        {
            WendellBloonsOld = HeliumHelperMod.WendellBloons;

            scrollPanel.ScrollContent.gameObject.DestroyAllChildren();

            foreach (string id in WendellBloonsOld.Keys.OrderBy(id => Game.instance.model.GetBloon(id).danger))
            {
                SpawnBloonButton button = SpawnBloonButton.Create(id);
                instance.scrollPanel.AddScrollContent(button);

                yield return null;
            }

            updateToken = null;

            yield return null;
        }

        void Update()
        {
            if(!readyForUpdate)
            {
                return;
            }

            if (tsm.selectedTower != null)
            {
                if (tsm.selectedTower.tower.towerModel.baseId == ModContent.TowerID<Wendell>())
                {
                    matchLocalPosition.offset.x = instance.tsm.IsOpenedRight() ? -1465 : 2065;
                }
                else
                {
                    Close();
                }
            }
        }

        public static readonly string TamedBloons = ModContent.Localize<HeliumHelperMod>(nameof(TamedBloons), "Tamed Bloons");

        public static IEnumerator Create()
        {
            var rect = InGame.instance.mapRect;

            var mainPanel = rect.gameObject.AddModHelperPanel(new("SpawnBloonsMenu", 1780, 2160), VanillaSprites.MainBgPanel);

            instance = mainPanel.AddComponent<WendellBloonsMenu>();
            instance.mainPanel = mainPanel;

            instance.tsm = TowerSelectionMenu.instance;

            var matchLocalPosition = instance.mainPanel.AddComponent<MatchLocalPosition>();
            matchLocalPosition.transformToCopy = instance.tsm.transform;
            matchLocalPosition.offset.x -= instance.tsm.IsOpenedRight() ? 1465 : -2065;
            matchLocalPosition.offset.y -= 430;

            instance.matchLocalPosition = matchLocalPosition;


            instance.mainPanel.AddText(new("Title", -25, 825, 1050, 200), TamedBloons, 125);

            instance.mainPanel.AddButton(new("CloseBtn", -890, 1080, 250), VanillaSprites.CloseBtn, new Action(instance.Close));

            Color textColor = new Color32(169, 129, 73, 255);

            var scrollPanel = instance.mainPanel.AddScrollPanel(new("ScrollPanel", -25, -175, 1480, 1800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel);

            scrollPanel.ScrollContent.RemoveComponent<VerticalLayoutGroup>();

            yield return null;

            GridLayoutGroup layoutGroup = scrollPanel.ScrollContent.gameObject.AddComponent<GridLayoutGroup>();

            layoutGroup.SetPadding(80);
            layoutGroup.spacing = new(80, 80);

            layoutGroup.cellSize = new(350, 350);

            instance.scrollPanel = scrollPanel;
            //layoutGroup.cellSize = new(350, 525);

            foreach (string bloonId in HeliumHelperMod.WendellBloons.Keys.OrderBy(id => Game.instance.model.GetBloon(id).danger))
            {
                SpawnBloonButton button = SpawnBloonButton.Create(bloonId);
                instance.scrollPanel.AddScrollContent(button);

                yield return null;
            }


            var addAllBloons = instance.mainPanel.AddButton(new("AddAllBloonsBtn", -740, 725, 150), VanillaSprites.AddMoreBtn, new Action(() => 
            {
                foreach(var id in InGame.instance.GetGameModel().bloonsByName.Keys)
                {
                    HeliumHelperMod.WendellBloons.TryAdd(id, 0);
                    HeliumHelperMod.WendellBloons[id] += 1;
                }

                if (instance.updateToken != null)
                {
                    MelonCoroutines.Stop(instance.updateToken);
                }

                instance.updateToken = MelonCoroutines.Start(instance.UpdateButtons());
            }));

            if (InGame.instance.GetGameModel().gameMode != GameModeType.Sandbox)
            {
                addAllBloons.gameObject.SetActive(false);
            }

            instance.readyForUpdate = true;

            yield return null;
        }
    }
}
