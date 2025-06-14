using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace OuterRimMandalore
{
    public class IncidentWorker_HonourRaid : IncidentWorker_PawnsArrive
    {
        Predicate<Faction> friendlyMandoValidation = delegate (Faction x)
        {
            if (x.def == OuterRimMandaloreDefOf.OuterRim_DeathWatch)
            {
                return true;
            }
            return false;
        };

        public override bool CanFireNowSub(IncidentParms parms)
        {
            if (!OuterRimMandaloreMod.settings.honourRaids)
            {
                return false;
            }

            var friendlyMandalorians = Find.FactionManager.AllFactions.Where(x => friendlyMandoValidation(x) && x.RelationKindWith(Faction.OfPlayer) != FactionRelationKind.Hostile);
            if (!friendlyMandalorians.Any())
            {
                return false;
            }
            if (!FactionCanBeGroupSource(friendlyMandalorians.First(), (Map)parms.target))
            {
                return false;
            }
            if (Find.FactionManager.FirstFactionOfDef(OuterRimMandaloreDefOf.OuterRim_DeathWatch_Honour) == null)
            {
                return false;
            }
            return true;
        }

        public override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            var friendlyMandalorians = Find.FactionManager.AllFactions.Where(x => friendlyMandoValidation(x) && x.RelationKindWith(Faction.OfPlayer) != FactionRelationKind.Hostile);
            var friendlyFaction = friendlyMandalorians.RandomElement();
            var enemyFaction = Find.FactionManager.FirstFactionOfDef(FactionDef.Named(friendlyFaction.def.defName + "_Honour"));

            var points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
            if (points < 300)
            {
                points = 300;
            }
            if (points > 5000)
            {
                points = 5000;
            }

            var raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            var raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;

            var enemyParms = new IncidentParms();
            enemyParms.target = parms.target;
            enemyParms.faction = enemyFaction;
            enemyParms.points = points;
            enemyParms.raidStrategy = raidStrategy;
            enemyParms.raidArrivalMode = raidArrivalMode;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out enemyParms.spawnCenter, map, CellFinder.EdgeRoadChance_Friendly))
            {
                return false;
            }

            var enemies = SpawnRaid(enemyParms, out List<TargetInfo> targetInfosEnemies);
            if (!enemies.Any() || !targetInfosEnemies.Any())
            {
                return false;
            }

            var totalTargets = targetInfosEnemies.ListFullCopy();
            Find.LetterStack.ReceiveLetter("OuterRim.Mandalore.HonourRaid".Translate(), "OuterRim.Mandalore.HonourRaidDesc".Translate(friendlyFaction.Named("friendlyFACTION")), LetterDefOf.ThreatBig, totalTargets);
            return true;
        }

        private List<Pawn> SpawnRaid(IncidentParms parms, out List<TargetInfo> targetInfos)
        {
            PawnGroupKindDef combat = PawnGroupKindDefOf.Combat;
            parms.raidStrategy.Worker.TryGenerateThreats(parms);
            List<Pawn> list = parms.raidStrategy.Worker.SpawnThreats(parms);
            if (list == null)
            {
                list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms)).ToList();
                if (list.Count == 0)
                {
                    Log.Error("Got no pawns spawning raid from parms " + parms);
                    targetInfos = null;
                    return list;
                }
                parms.raidArrivalMode.Worker.Arrive(list, parms);
            }

            List<TargetInfo> list2 = new List<TargetInfo>();
            if (parms.pawnGroups != null)
            {
                List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
                List<Pawn> list4 = list3.MaxBy((List<Pawn> x) => x.Count);
                if (list4.Any())
                {
                    list2.Add(list4[0]);
                }
                for (int i = 0; i < list3.Count; i++)
                {
                    if (list3[i] != list4 && list3[i].Any())
                    {
                        list2.Add(list3[i][0]);
                    }
                }
            }
            else if (list.Any())
            {
                foreach (Pawn item in list)
                {
                    list2.Add(item);
                }
            }
            parms.raidStrategy.Worker.MakeLords(parms, list);
            targetInfos = list2;
            return list;
        }
    }
}
