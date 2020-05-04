using HarmonyLib;

using System.Reflection;
using System.Xml;

using TaleWorlds.Core;

namespace Aragas.Core
{
    [HarmonyPatch(typeof(ItemObject))]
    [HarmonyPatch("Deserialize")]
    public class ItemObjectPatch
    {
        private static MethodInfo SetItemFlagsMethod { get; } = typeof(ItemObject).GetProperty("ItemFlags").SetMethod;

        public static void Postfix(ItemObject __instance, XmlNode node)
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
            var flags = itemObject.ItemFlags | ItemFlags.Civilian;
            SetItemFlagsMethod.Invoke(itemObject, new object[] { flags });
        }
    }
}