using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Xml;

using TaleWorlds.Core;

namespace Aragas.CastleTownWithHorse.Patches
{
    public static class ItemObjectPatch
    {
        private delegate void SetItemFlagsDelegate(object instance, ItemFlags itemFlags);

        private static readonly SetItemFlagsDelegate? SetItemFlags =
            AccessTools2.GetDelegateObjectInstance<SetItemFlagsDelegate>(AccessTools.PropertySetter(typeof(ItemObject), "ItemFlags"));

        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(ItemObject), "Deserialize"),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(ItemObjectPatch), nameof(Postfix))));
        }

        private static void Postfix(ItemObject __instance, XmlNode node)
        {
            switch (node.Name)
            {
                case "Item":
                    if (__instance.Type == ItemObject.ItemTypeEnum.HorseHarness && __instance.HasArmorComponent)
                    {
                        if (__instance.ArmorComponent.MaterialType == ArmorComponent.ArmorMaterialTypes.Cloth ||
                            __instance.ArmorComponent.MaterialType == ArmorComponent.ArmorMaterialTypes.Leather)
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