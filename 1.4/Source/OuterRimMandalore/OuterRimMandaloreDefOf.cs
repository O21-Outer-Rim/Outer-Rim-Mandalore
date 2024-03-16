using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace OuterRimMandalore
{
    [DefOf]
    public static class OuterRimMandaloreDefOf
    {
        static OuterRimMandaloreDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OuterRimMandaloreDefOf));
        }

        public static FactionDef OuterRim_DeathWatch;
        public static FactionDef OuterRim_DeathWatch_Honour;

        public static HistoryEventDef OuterRim_HonourEarned;
    }
}
