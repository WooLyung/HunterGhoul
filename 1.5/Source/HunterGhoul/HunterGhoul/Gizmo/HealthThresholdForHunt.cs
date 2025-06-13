using UnityEngine;
using Verse;

namespace HunterGhoul
{
    public class HealthThresholdForHunt : Gizmo_Slider
    {
        private HediffComp_GhoulHuntData comp;

        public HealthThresholdForHunt(HediffComp_GhoulHuntData comp)
        {
            this.comp = comp;
        }

        protected override float Target {
            get => comp.healthThresholdForHunt;
            set => comp.healthThresholdForHunt = value;
        }

        protected override float ValuePercent => comp.healthThresholdForHunt;

        protected override bool IsDraggable => true;

        protected override string Title => "HealthThresholdForHuntTitle".Translate();

        protected override string GetTooltip() => "";

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            if (Mouse.IsOver(rect))
            {
                TipSignal tip = "HealthThresholdForHuntTip".Translate((int)(comp.healthThresholdForHunt * 100));
                TooltipHandler.TipRegion(rect, tip);
            }
            return base.GizmoOnGUI(topLeft, maxWidth, parms);
        }
    }
}
