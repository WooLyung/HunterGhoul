using RimWorld;
using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace HunterGhoul
{
    public class JobGiver_GhoulHunt : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn ghoul)
        {
            if (ghoul.mutant.Hediff == null)
                return null;
            var comp = ghoul.mutant.Hediff.TryGetComp<HediffComp_GhoulHuntData>();
            if (comp == null || !comp.isHunter || ghoul.health.summaryHealth.SummaryHealthPercent < comp.healthThresholdForHunt)
                return null;

            var targets = ghoul.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt)
                .Where(item => item.target.Thing is Verse.Pawn && canGiveJobOnPawn(ghoul, item.target.Thing as Verse.Pawn));
            var target = !targets.Any() ? null : targets.MinBy(item => item.target.Thing.Position.DistanceToSquared(ghoul.Position)).target.Thing;
            if (target == null || !(target is Verse.Pawn))
                return null;

            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
            if (job != null)
            {
                job.killIncappedTarget = true;
                return job;
            }
            return null;
        }

        private bool canGiveJobOnPawn(Verse.Pawn ghoul, Verse.Pawn target)
        {
            if (!target.AnimalOrWildMan())
                return false;
            try
            {
                if (ghoul.playerSettings.AreaRestrictionInPawnCurrentMap != null && !ghoul.playerSettings.AreaRestrictionInPawnCurrentMap[target.Position])
                    return false;
            }
            catch (Exception) { }
            if (!ghoul.Map.reachability.CanReach(ghoul.Position, target, PathEndMode.Touch, TraverseParms.For(ghoul)))
                return false;
            return true;
        }
    }
}
