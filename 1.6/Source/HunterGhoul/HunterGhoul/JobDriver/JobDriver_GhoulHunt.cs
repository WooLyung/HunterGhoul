using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace HunterGhoul
{
    public class JobDriver_GhoulHunt : JobDriver_AttackMelee
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddFinishAction(delegate
            {
                if (pawn.IsPlayerControlled && pawn.Drafted && !base.job.playerInterruptedForced)
                {
                    Thing targetThingA = base.TargetThingA;
                    if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
                    {
                        foreach (IntVec3 item in GenRadial.RadialCellsAround(base.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
                        {
                            if (item.InBounds(base.Map))
                            {
                                foreach (Thing thing2 in item.GetThingList(base.Map))
                                {
                                    if (thing2.def == base.TargetThingA.def && pawn.CanReach(thing2, PathEndMode.Touch, Danger.Deadly) && pawn.jobs.jobQueue.Count == 0)
                                    {
                                        Job job = base.job.Clone();
                                        job.targetA = thing2;
                                        pawn.jobs.jobQueue.EnqueueFirst(job);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            });
            yield return Toils_General.DoAtomic(delegate
            {
                if (job.targetA.Thing is Pawn pawn)
                {
                    bool num = pawn.Downed && base.pawn.mindState.duty != null && base.pawn.mindState.duty.attackDownedIfStarving && base.pawn.Starving();
                    CompActivity comp;
                    bool flag = ModsConfig.AnomalyActive && pawn.TryGetComp<CompActivity>(out comp) && comp.IsDormant;
                    if (num || flag)
                    {
                        job.killIncappedTarget = true;
                    }
                }
            });
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate {
                Thing thing = job.GetTarget(TargetIndex.A).Thing;
                pawn.meleeVerbs.TryMeleeAttack(thing, job.verbToUse);
            }).FailOnDespawnedOrNull(TargetIndex.A);
        }
    }
}
