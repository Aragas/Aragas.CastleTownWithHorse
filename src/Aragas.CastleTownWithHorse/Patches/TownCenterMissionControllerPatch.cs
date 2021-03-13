﻿using HarmonyLib;

using SandBox;
using SandBox.Source.Missions;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using TaleWorlds.Library;

namespace Aragas.CastleTownWithHorse.Patches
{
    public static class TownCenterMissionControllerPatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(TownCenterMissionController), "AfterStart"),
                transpiler: new HarmonyMethod(AccessTools.Method(typeof(TownCenterMissionControllerPatch), nameof(Transpiler))));
        }

        // set noHorses to false
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = instructions.ToList();

            IEnumerable<CodeInstruction> ReturnDefault(string reason)
            {
                return instructionsList.AsEnumerable();
            }

            var spawnPlayerMethod = AccessTools.Method(typeof(MissionAgentHandler), "SpawnPlayer");
            if (spawnPlayerMethod is null)
                return ReturnDefault("Missing method SpawnPlayer in MissionAgentHandler");

            var spawnPlayerParameters = spawnPlayerMethod.GetParameters();
            var noHorseParamIndex = spawnPlayerParameters.FindIndex(p => p.Name == "noHorses");

            if (noHorseParamIndex == -1)
                return ReturnDefault("Missing parameter 'noHorse' in method SpawnPlayer");

            var spawnPlayerIndex = -1;
            for (var i = 0; i < instructionsList.Count; i++)
            {
                if (!instructionsList[i].Calls(spawnPlayerMethod))
                    continue;

                spawnPlayerIndex = i;
                break;
            }

            if (spawnPlayerIndex == -1)
                return ReturnDefault("Pattern not found");

            var opCode = instructionsList[spawnPlayerIndex - spawnPlayerParameters.Length + noHorseParamIndex];
            opCode.opcode = OpCodes.Ldc_I4_0;

            return instructionsList;
        }
    }
}