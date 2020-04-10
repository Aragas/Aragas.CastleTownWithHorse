using HarmonyLib;

using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
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

        protected override void OnSubModuleLoad()
        {
            //var manager = MBObjectManager.Instance;
            base.OnSubModuleLoad();
        }

        public override bool DoLoading(Game game)
        {
            var manager = MBObjectManager.Instance;
			return base.DoLoading(game);
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            var manager = MBObjectManager.Instance;
            /*
            var items = manager.ObjectTypeRecords.FirstOrDefault(r => r.ElementListName == "Items");
            if (items != null)
            {
                foreach (ItemObject item in items)
                {
                    if(item)

                    item.ItemFlags |= ItemFlags.Civilian;
                }
            }
            */

			base.OnGameStart(game, gameStarterObject);
        }
    }
}