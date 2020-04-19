using RimWorld;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using UnityEngine;

using Verse;
using Verse.Sound;

namespace LTF_Slug
{
    // Main
    [StaticConstructorOnStartup]
    public class Comp_LTF_MindFlaySpot : ThingComp
    {
        // Work base
        Building building = null;
        Vector3 buildingPos;

        Map myMap = null;

        Pawn Initiator = null;
        float Range = 0f;
        int FlayageNum = 0;

        bool drawOverlay = true;
        bool drawUnderlay = true;

        MyGfx.ClosestColor closestColor = MyGfx.ClosestColor.blue;

        // Debug 
        public bool gfxDebug = false;
        public bool prcDebug = false;
        public bool myDebug = true;

        // Props
        public CompProperties_LTF_MindFlaySpot Props
        {
            get
            {
                return (CompProperties_LTF_MindFlaySpot)props;
            }
        }
        private void SetRange()
        {
            Range = Props.range;
        }
        public void SetPawn(Pawn pawn = null)
        {
            Initiator = pawn;
        }
        private string DumpProps
        {
            get
            {
                return ("Range: " + Range + "; Pawn: "+Initiator.Label );
            }
        }

        // Overrides
        public override void PostDraw()
        {
            base.PostDraw();

            Material overlayMat = null;
            Material underlayMat = null;

            if (drawUnderlay)
            {
                if(closestColor == MyGfx.ClosestColor.blue)
                    underlayMat = MyGfx.BlueUnderlayM;
                if (closestColor == MyGfx.ClosestColor.orange)
                    underlayMat = MyGfx.OrangeUnderlayM;
                if (closestColor == MyGfx.ClosestColor.purple)
                    underlayMat = MyGfx.PurpleUnderlayM;

                float opacity = GfxEffects.VanillaPulse(parent);
                // (1 - value) bc we want to rotate counter clockwise
                float angle = (1 - GfxEffects.LoopAroundOneSuperSlow(parent)) * 360;

                GfxEffects.DrawTickRotating(parent, underlayMat, 0, 0, 2*Range, angle, opacity, GfxEffects.Layer.under, false);

            }
            if (drawOverlay)
            {
                if (closestColor == MyGfx.ClosestColor.blue)
                    overlayMat = MyGfx.BlueOverlayM;
                if (closestColor == MyGfx.ClosestColor.orange)
                    overlayMat = MyGfx.OrangeOverlayM;
                if (closestColor == MyGfx.ClosestColor.purple)
                    overlayMat = MyGfx.PurpleOverlayM;

                float opacity = GfxEffects.PulseFactorOne(parent);
                // (1 - value) bc we want to rotate counter clockwise
                float angle = (1 - GfxEffects.LoopAroundOneNormal(parent)) * 360;

                GfxEffects.DrawTickRotating(parent, overlayMat, 0, 0, 2*Range, angle, opacity, GfxEffects.Layer.under, false);
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            //Building
            building = (Building)parent;
            buildingPos = building.DrawPos;
            myMap = building.Map;

            closestColor = GfxEffects.ClosestColor(building, myDebug);

            SetRange();
        }
        public override void CompTickRare()
        {
            base.CompTickRare();

            //Applying torment
            List<Pawn> affectedPawnList = new List<Pawn> { };
            affectedPawnList = ToolsCell.GetPawnsInRadius(buildingPos.ToIntVec3(), Range, myMap);

            foreach (Pawn curPawn in affectedPawnList)
            {
                // Slugs are immune to this
                if (curPawn.IsSlug())
                    continue;

                // Add psychicSensitivity * SocialImpact * SocialSkill fight here
                if (ToolsPawn.ApplyHediffOnBodyPartTag(curPawn, BodyPartTagDefOf.ConsciousnessSource, MyXmlDef.MindFlayedHediff, prcDebug))
                {
                    Thought_Memory MindFlayed = (Thought_Memory)ThoughtMaker.MakeThought(MyXmlDef.MindFlayedThought);
                    curPawn.needs.mood.thoughts.memories.TryGainMemory(MindFlayed, Initiator);
                    GfxEffects.ThrowMindFlayMote(curPawn.Position.ToVector3(), myMap);
                    FlayageNum++;
                }
            }

            //Checking if Initiator is not mad or downed or sleepin or on fire
            if (Initiator.InMentalState || Initiator.Downed || Initiator.IsSleepingOrOnFire())
                building.Destroy();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_References.Look(ref Initiator, "Initiator");
            Scribe_Values.Look(ref FlayageNum, "FlayageNum");
        }

        public override string CompInspectStringExtra()
        {
            string text = base.CompInspectStringExtra();
            string result = string.Empty;

            result += "Flayage inflicted count: " + FlayageNum;
            if (!text.NullOrEmpty())
            {
                result = "\n" + text;
            }

            return result;
        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Prefs.DevMode)
            {
                // Debug process
                yield return new Command_Action
                {
                    icon = ((prcDebug) ? (MyGizmo.DebugOnGz) : (MyGizmo.DebugOffGz)),
                    defaultLabel = "prcDebug: " + prcDebug,
                    defaultDesc = "process debug\n" + DumpProps,
                    action = delegate
                    {
                        prcDebug = !prcDebug;
                    }
                };
                // Debug gfx
                yield return new Command_Action
                {
                    icon = ((gfxDebug) ? (MyGizmo.DebugOnGz) : (MyGizmo.DebugOffGz)),
                    defaultLabel = "gfxDebug: " + gfxDebug,
                    defaultDesc = "gfx debug",
                    action = delegate
                    {
                        gfxDebug = !gfxDebug;
                    }
                };
            }
            if (gfxDebug)
            {
                yield return new Command_Action
                {
                    defaultLabel = "under " + drawUnderlay + "->" + !drawUnderlay,
                    action = delegate
                    {
                        drawUnderlay = !drawUnderlay;
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "over " + drawOverlay + "->" + !drawOverlay,
                    action = delegate
                    {
                        drawOverlay = !drawOverlay;
                    }
                };
                
            }

        }
        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();
            // Flickering line between spot and twin
            GenDraw.DrawLineBetween(parent.TrueCenter(), Initiator.TrueCenter(), SimpleColor.Magenta);


            if (Range > 0f)
            {
                // Cannot draw radius ring of radius 140.7: not enough squares in the precalculated list.
                if (Range < GenRadial.MaxRadialPatternRadius)
                    GenDraw.DrawRadiusRing(this.parent.Position, Range);
            }
        }
    }
}
