using Verse;

namespace HunterGhoul
{
    public class HediffComp_GhoulHuntData : HediffComp
    {
        public bool isHunter = true;
        public float healthThresholdForHunt = 0.7f;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref isHunter, "isHunter", true);
            Scribe_Values.Look(ref healthThresholdForHunt, "healthThresholdForHunt", 0.7f);
        }
    }
}
