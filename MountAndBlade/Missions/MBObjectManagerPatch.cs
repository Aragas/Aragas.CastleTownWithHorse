using System;
using System.Linq;
using System.Xml;
using HarmonyLib;
using TaleWorlds.Core;

namespace Aragas.MountAndBlade.Missions
{
    /// <summary>
    /// Sets Cloth and Leather based HorseHarness Civilian attribute to true
    /// </summary>
    [HarmonyPatch(typeof(MBObjectManager))]
    [HarmonyPatch("LoadXml")]
    public class MBObjectManagerPatch
    {
        public static void Prefix(XmlDocument doc, Type typeOfGameMenusCallbacks)
        {
            if (doc?.DocumentElement?.Name == "Items")
            {
                foreach (var itemElement in doc.DocumentElement.ChildNodes.OfType<XmlElement>())
                {
                    var type = itemElement.Attributes.GetNamedItem("Type")?.InnerText;
                    if (string.IsNullOrEmpty(type) || type != "HorseHarness")
                        continue;

                    var itemComponents = itemElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Name == "ItemComponent");
                    if (itemComponents == null)
                        continue;

                    var armor = itemComponents.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Name == "Armor");
                    if (armor == null)
                        continue;

                    var materialType = armor.Attributes.GetNamedItem("material_type")?.InnerText;
                    if (string.IsNullOrEmpty(materialType) || (materialType != "Leather" && materialType != "Cloth"))
                        continue;

                    var flags = itemElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Name == "Flags");
                    if (flags == null)
                        continue;
                    //var flags = itemElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Name == "Flags") ??
                    //              (XmlElement) itemElement.AppendChild(doc.CreateNode(XmlNodeType.Element, "Flags", null));

                    var civilian = flags.Attributes.GetNamedItem("Civilian");
                    if (civilian == null)
                        flags.SetAttribute("Civilian", "true");
                    else
                    {
                        if (civilian.Value != "true")
                            civilian.Value = "true";
                    }
                }
            }
        }
    }
}