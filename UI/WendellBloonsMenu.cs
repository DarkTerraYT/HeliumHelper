using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using HeliumHelper.WendellTower;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Il2CppInterop.Runtime.Attributes;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HeliumHelper.UI
{
    [RegisterTypeInIl2Cpp(false)]
    public class WendellBloonsMenu : MonoBehaviour
    {
        public static WendellBloonsMenu instance;

        public GridLayoutGroup layoutGroup;

        public static bool autoSendEnabled = false;

        public ModHelperPanel mainPanel;

        public ModHelperButton autoSendBtn;

        internal object updateToken;

        public ModHelperScrollPanel scrollPanel;

        public Dictionary<string, int> WendellBloonsOld = [];
        TowerSelectionMenu tsm;

        MatchLocalPosition matchLocalPosition;

        public static float nextAutoSendTime = 0;

        bool readyForUpdate = false;

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

            if(!InGame.instance.GetTowers().Any(twr => twr.towerModel.baseId == ModContent.TowerID<Wendell>()))
            {
                Close();
            }

            if (tsm.selectedTower != null)
            {
                if (tsm.selectedTower.tower.towerModel.baseId == ModContent.TowerID<Wendell>())
                {
                    gameObject.Show();

                    if (tsm.selectedTower.tower.towerModel.tiers[0] >= 5)
                    {
                        autoSendBtn.SetActive(true);

                        string btnGuid = VanillaSprites.GreenBtn;
                        if (!autoSendEnabled)
                        {
                            btnGuid = VanillaSprites.RedBtn;
                        }

                        autoSendBtn.Image.SetSprite(btnGuid);
                    }
                    else
                    {
                        autoSendBtn.SetActive(false);
                    }
                    matchLocalPosition.offset.x = instance.tsm.IsOpenedRight() ? -1465 : 2065;
                }
                else
                {
                    Close();
                }
            }
            else
            {
                gameObject.Hide();
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


            var addAllBloons = instance.mainPanel.AddButton(new("AddAllBloonsBtn", -740, 575, 150), VanillaSprites.AddMoreBtn, new Action(() => 
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

            instance.autoSendBtn = instance.mainPanel.AddButton(new("AddAllBloonsBtn", -740, 725, 150), VanillaSprites.RedBtn, new Action(() => autoSendEnabled = !autoSendEnabled));

            instance.autoSendBtn.AddImage(new("Icon", 125), VanillaSprites.RaceIcon);

            instance.readyForUpdate = true;

            yield return null;
        }
    }
}
