using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace Aragas.MountAndBlade
{
	public class CastleTownWithHorseSubModule : MBSubModuleBase
	{
		public CastleTownWithHorseSubModule()
		{
			try
			{
				var harmony = new Harmony("org.aragas.bannerlord.castletownwithhorse");
				harmony.PatchAll(typeof(CastleTownWithHorseSubModule).Assembly);
			}
			catch (Exception ex)
			{
				// TODO: Find a logger
			}
		}
	}
}