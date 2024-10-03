using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace HunterGhoul
{
    [HarmonyPatch]
    public class GetGizmos_Patch
    {
        static MethodBase TargetMethod() => AccessTools.Method(typeof(Pawn), "GetGizmos");

        static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            if (__instance.IsColonyMutant && __instance.IsGhoul && __instance.mutant.Hediff != null)
            {
                var comp = __instance.mutant.Hediff.TryGetComp<HediffComp_GhoulHuntData>();
                if (comp != null)
                {
                    Command_Toggle command_toggle = new Command_Toggle();
                    command_toggle.isActive = () => comp.isHunter;
                    command_toggle.toggleAction = delegate
                    {
                        comp.isHunter = !comp.isHunter;
                    };
                    command_toggle.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt");
                    command_toggle.turnOnSound = SoundDefOf.DraftOn;
                    command_toggle.turnOffSound = SoundDefOf.DraftOff;
                    command_toggle.defaultLabel = "IsHunterGhoulLabel".Translate();
                    command_toggle.defaultDesc = "IsHunterGhoulDesc".Translate();

                    yield return command_toggle;
                    yield return new HealthThresholdForHunt(comp);
                }
            }
   
            foreach (Gizmo gizmo in __result)
                yield return gizmo;
        }
    }
}
