using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using TabulaRasa;

namespace OuterRimMandalore
{
    public class OuterRimMandaloreMod : Mod
    {
        public static OuterRimMandaloreMod mod;
        public static OuterRimMandaloreSettings settings;

        public Vector2 optionsScrollPosition;
        public float optionsViewRectHeight;

        internal static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public OuterRimMandaloreMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<OuterRimMandaloreSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            Log.Message($":: Outer Rim - Mandalore :: ".Colorize(Color.cyan) + $"{CurrentVersion} ::");

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            Harmony harmony = new Harmony("Neronix17.OuterRim.Mandalore");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void DoOptionsCategoryContents(Listing_Standard listing)
        {
            listing.GapLine();
            listing.Note("Mandalore", GameFont.Medium);
            listing.GapLine();
            listing.CheckboxEnhanced("Enable Honour Raids", "If enabled, neutral and friendly mandalorian factions will send honour raids occasionally, every honour raid combatant killed will gain goodwill with the faction that sent it. Capturing them and sending them on their way when healed on the other hand will do nothing for goodwill.", ref settings.honourRaids);
        }
    }
}
