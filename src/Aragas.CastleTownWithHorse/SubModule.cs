using Aragas.CastleTownWithHorse.Patches;

using HarmonyLib;

using TaleWorlds.MountAndBlade;

namespace Aragas.CastleTownWithHorse
{
    public class SubModule : MBSubModuleBase
	{
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var harmony = new Harmony("org.aragas.bannerlord.castletownwithhorse");
            ItemObjectPatch.Patch(harmony);
            TownCenterMissionControllerPatch.Patch(harmony);
        }
    }
}