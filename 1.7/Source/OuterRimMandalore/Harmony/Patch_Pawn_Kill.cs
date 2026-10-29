using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace OuterRimMandalore
{
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Patch_Pawn_Kill
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance)
        {
            if (!PawnGenerator.IsBeingGenerated(__instance) && Current.ProgramState == ProgramState.Playing && !__instance.IsSlaveOfColony && !__instance.HostileTo(Faction.OfPlayer))
            {
                if (__instance.Faction?.def == OuterRimMandaloreDefOf.OuterRim_DeathWatch_Honour)
                {
                    Faction.OfPlayer.TryAffectGoodwillWith(Find.FactionManager.FirstFactionOfDef(OuterRimMandaloreDefOf.OuterRim_DeathWatch), 2, false, false, OuterRimMandaloreDefOf.OuterRim_HonourEarned);
                }
            }
        }
    }
}
