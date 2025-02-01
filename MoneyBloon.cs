using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HeliumHelper
{
    public class MoneyBloon : ModBloon
    {
        public override string BaseBloon => BloonType.sPink;

        public override string Icon => base.Icon + "Icon";

        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.leakDamage = 1;
            bloonModel.maxHealth = 15;

            bloonModel.GetBehavior<DistributeCashModel>().cash = 50;
        }
    }

    public class MoneyBloonDisplay : ModBloonDisplay<MoneyBloon>
    {
        public override string BaseDisplay => GetBloonDisplay("Pink");

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "MoneyBloon");
        }
    }
    public class MoneyMoab : ModBloon
    {
        public override string BaseBloon => BloonType.sMoab;

        public override IEnumerable<string> DamageStates => [];

        public override string Icon => base.Icon + "Icon";

        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.leakDamage = 1;
            bloonModel.maxHealth = 99999999;

            bloonModel.GetBehavior<DistributeCashModel>().cash = 620;

            bloonModel.AddToChildren<MoneyBloon>(4);
        }
    }

    public class MoneyMoabDisplay : ModBloonDisplay<MoneyMoab>
    {
        public override string BaseDisplay => GetBloonDisplay("Moab");

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
            SetMeshOutlineColor(node, new Color32(65, 143, 39, 255));
        }
    }
}
