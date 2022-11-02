using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Xml;

using TaleWorlds.Core;

namespace Aragas.CastleTownWithHorse.Patches
{
    public static class ItemObjectPatch
    {
        private delegate void SetItemFlagsDelegate(ItemObject instance, ItemFlags itemFlags);
        private static readonly SetItemFlagsDelegate? SetItemFlags =
            AccessTools2.GetPropertySetterDelegate<SetItemFlagsDelegate>(typeof(ItemObject), "ItemFlags");

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools2.Method(typeof(ItemObject), "Deserialize"),
                postfix: new HarmonyMethod(typeof(ItemObjectPatch), nameof(Postfix)));
        }

        private static void Postfix(ItemObject __instance, XmlNode node)
        {
            switch (node.Name)
            {
                case "Item":
                    if (__instance.Type == ItemObject.ItemTypeEnum.HorseHarness && __instance.HasArmorComponent)
                    {
                        if (__instance.ArmorComponent.MaterialType is ArmorComponent.ArmorMaterialTypes.Cloth or ArmorComponent.ArmorMaterialTypes.Leather)
                            SetCivilian(__instance);
                    }
                    break;
            }
        }

        private static void SetCivilian(ItemObject itemObject)
        {
            if (SetItemFlags is not null)
            {
                SetItemFlags(itemObject, itemObject.ItemFlags | ItemFlags.Civilian);
            }
        }
    }
}