using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HeliumHelper.UI
{
    [RegisterTypeInIl2Cpp(false)]
    internal class WendellBloonsButton : MonoBehaviour
    {
        public static WendellBloonsButton instance;

        public void Close()
        {
            if(gameObject)
            {
                gameObject.Destroy();
                instance = null;
            }
        }

        public static void Create(TowerSelectionMenu tsm)
        {
            if(instance != null)
            {
                instance.Close();
            }

            if(WendellBloonsMenu.instance != null)
            {
                WendellBloonsMenu.instance.gameObject.SetActive(true);
                return;
            }

            float x = tsm.IsOpenedRight() ? -620 : 620;

            var btn = ModHelperButton.Create(new("Open", x, -935, 350), VanillaSprites.BlueBtn, new Action(() => 
            {
                MenuManager.instance.buttonClickSound.Play("ClickSounds");
                MelonCoroutines.Start(WendellBloonsMenu.Create());
                instance.Close();
            }));

            btn.AddImage(new("Icon", 300), ModContent.GetTextureGUID<HeliumHelperMod>("MendedHeart-Icon"));

            instance = btn.AddComponent<WendellBloonsButton>();

            tsm.gameObject.AddModHelperComponent(btn);
        }
    }
}
