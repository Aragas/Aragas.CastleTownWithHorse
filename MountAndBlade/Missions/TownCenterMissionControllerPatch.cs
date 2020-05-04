using HarmonyLib;

using SandBox.Source.Missions;

using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using TaleWorlds.MountAndBlade;

namespace Aragas.MountAndBlade.Missions
{
    [HarmonyPatch(typeof(TownCenterMissionController))]
    [HarmonyPatch("AfterStart")]
    public class TownCenterMissionControllerPatch
    {
        private static PropertyInfo Operand { get; } = typeof(Mission).GetProperty("DoesMissionRequireCivilianEquipment");

        // set noHorses to false
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var flag = false; // Mission.get_DoesMissionRequireCivilianEquipment is found
            var flag2 = false; // perform operation only once
            foreach (var instruction in instructions)
            {
                if (!flag2 && flag && instruction.opcode == OpCodes.Ldc_I4_1)
                {
                    // replace true (Ldc_I4_1) with false (Ldc_I4_0)
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0, instruction.operand);
                    flag = false;
                    flag2 = true;
                    continue;
                }

                // look for Mission.get_DoesMissionRequireCivilianEquipment
                if (!flag2 && instruction.opcode == OpCodes.Callvirt && instruction.operand == Operand.GetMethod)
                    flag = true;

                yield return instruction;
            }
        }
    }
}