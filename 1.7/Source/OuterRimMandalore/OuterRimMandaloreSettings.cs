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
    public class OuterRimMandaloreSettings : ModSettings
    {
        public bool verboseLogging = false;

        public bool honourRaids = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref honourRaids, "honourRaids", true);
        }
    }
}
