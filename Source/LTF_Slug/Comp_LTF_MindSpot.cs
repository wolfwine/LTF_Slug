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
    public class Comp_LTF_MindSpot : ThingComp
    {
        // things dependency
        Building building = null;
        Vector3 buildingPos;
        Map myMap = null;

        // calculated
        MyDefs.SpotKind spotKind = MyDefs.SpotKind.flay;
        HediffDef hediffDefToApply = MyDefs.MindFlayHediff;
        Material underlayMat = null;
        Material overlayMat = null;

        Pawn Initiator = null;
        float Range = 0f;
        int AffectedPawnsNum = 0;

        bool drawOverlay = true;
        bool drawUnderlay = true;

        MyGfx.ClosestColor closestColor = MyGfx.ClosestColor.blue;

        public bool EnableTiredness = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().TirednessOnOveruse;

        // Debug 
        public bool gfxDebug = false;
        public bool prcDebug = false;
        public bool myDebug = false;

        // Props
        public CompProperties_LTF_MindSpot Props
        {
            get
            {
                return (CompProperties_LTF_MindSpot)props;
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
                return 
                    "spotkind: " + spotKind +
                    ";closestColor: " + closestColor +
                    ";Color: " + building.DrawColor +

                    ";Humanlike: " + Props.affectsHumanlike +
                    ";Animals: " + Props.affectsAnimals +
                    ";Mechanoids" + Props.affectsMechanoids +
                    "\n"+
                    ";Colonists: " + Props.affectsColonists +
                    ";NeutralOrFriends: " + Props.affectsNeutralOrFriends +
                    ";Enemies: " + Props.affectsEnemies +
                    "\n" +
                    "; AffectedPawnsNum:"+ AffectedPawnsNum+
                    ((EnableTiredness) ?(";Props.hediffAppliedLimit: "+ Props.hediffAppliedLimit) :(""))+
                    ((EnableTiredness) ? (";ExceededLimit: " + IsLimitExceeded) : ("")) +
                    ";lifeSpan: " + Props.lifeSpan +
                    ";range: " + Props.range +
                    ";initiator: " + Initiator.Label;
            }
        }
        private bool HasFlaySpotDefName
        {
            get
            {
                return (building.def.defName == MyDefs.MindFlaySpotName);
            }
        }
        private bool HasFondleSpotDefName
        {
            get
            {
                return (building.def.defName == MyDefs.MindFondleSpotName);
            }
        }
        private void SetSpotKind()
        {
            if (HasFlaySpotDefName)
                spotKind = MyDefs.SpotKind.flay;
            else if (HasFondleSpotDefName)
                spotKind = MyDefs.SpotKind.fondle;
            else
            {
                spotKind = MyDefs.SpotKind.na;
                Tools.Warn("SetSpotKind - No valid spotKind, defname wont match ", myDebug);
            }
                
        }
        private bool IsFlaySpot
        {
            get
            {
                return spotKind == MyDefs.SpotKind.flay;
            }
        }
        private bool IsFondleSpot
        {
            get
            {
                return spotKind == MyDefs.SpotKind.fondle;
            }
        }
        private void SetHediffToApply()
        {
            if (IsFlaySpot)
                hediffDefToApply = MyDefs.MindFlayHediff;
            else if (IsFondleSpot)
                hediffDefToApply = MyDefs.MindFondleHediff;
            else
                Tools.Warn("SetHediffToApply - No valid spotKind ", myDebug);
        }
        private Material GetUnderlayMaterial
        {
            get
            {
                if (IsFlaySpot)
                switch ((int)closestColor)
                {
                    case (int)MyGfx.ClosestColor.blue:
                        return MyGfx.FlayBlueUnderlay;
                    case (int)MyGfx.ClosestColor.orange:
                        return MyGfx.FlayOrangeUnderlay;
                    case (int)MyGfx.ClosestColor.purple:
                        return MyGfx.FlayPurpleUnderlay;
                }
                else if (IsFondleSpot)
                switch ((int)closestColor)
                {
                    case (int)MyGfx.ClosestColor.blue:
                        return MyGfx.FondleBlueUnderlay;
                    case (int)MyGfx.ClosestColor.orange:
                        return MyGfx.FondleOrangeUnderlay;
                    case (int)MyGfx.ClosestColor.purple:
                        return MyGfx.FondlePurpleUnderlay;
                }

                Tools.Warn("pickUnderlayMat Unexpected spotkind", myDebug);
                return MyGfx.FondleBlueUnderlay;
            }
        }
        private Material GetOverlayMaterial
        {
            get
            {
                if (IsFlaySpot)
                {
                    return MyGfx.FlayOverlay;
                }
                else if (IsFondleSpot)
                switch ((int)closestColor) {
                    case (int)MyGfx.ClosestColor.blue:
                        return MyGfx.FondleBlueOverlay;
                    case (int)MyGfx.ClosestColor.orange:
                        return MyGfx.FondleOrangeOverlay;
                    case (int)MyGfx.ClosestColor.purple:
                        return MyGfx.FondlePurpleOverlay;
                }

                Tools.Warn("pickOverlayMat Unexpected spotkind", myDebug);
                return MyGfx.FondleBlueUnderlay;
            }
        }

        private void SetClosestColor()
        {
            closestColor = GfxEffects.ClosestColor(building, myDebug);
        }
        private void SetMaterials()
        {
            underlayMat = GetUnderlayMaterial;
            overlayMat = GetOverlayMaterial;
        }

        private bool IsLimitExceeded
        {
            get
            {
                if (!EnableTiredness)
                    return false;

                return (AffectedPawnsNum >= Props.hediffAppliedLimit);
            }
        }

        // Overrides
        public override void PostDraw()
        {
            base.PostDraw();

            if (drawUnderlay)
            {
                float opacity = GfxEffects.VanillaPulse(parent);
                // (1 - value) bc we want to rotate counter clockwise
                float angle = (1 - GfxEffects.LoopAroundOneSuperSlow(parent)) * 360;

                GfxEffects.DrawTickRotating(parent, underlayMat, 0, 0, 2*Range, angle, opacity, GfxEffects.Layer.under, false);
            }
            if (drawOverlay)
            {
                float opacity = GfxEffects.PulseFactorOne(parent);

                float angle = 0f;
                if(IsFlaySpot)
                    //clockwise
                    angle = GfxEffects.LoopAroundOneNormal(parent) *360;
                else
                    //anticlockwise
                    angle = (1 - GfxEffects.LoopAroundOneNormal(parent)) * 360;

                GfxEffects.DrawTickRotating(parent, overlayMat, 0, 0, 2*Range, angle, opacity, GfxEffects.Layer.under, false);
            }
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            //Building
            building = (Building)parent;
            buildingPos = building.DrawPos;
            myMap = building.Map;

            SetRange();
            SetSpotKind();
            // requires spotKind set
            SetHediffToApply();
            SetClosestColor();
            // requires closestColor & spotKind set
            SetMaterials();
        }
        public override void CompTickRare()
        {
            base.CompTickRare();

            //Applying torment
            List<Pawn> affectedPawnList = new List<Pawn> { };
            affectedPawnList = ToolsCell.GetPawnsInRadius(
                buildingPos.ToIntVec3(), Range, myMap,
                Props.affectsAnimals, Props.affectsHumanlike, Props.affectsMechanoids,
                Props.affectsColonists, Props.affectsNeutralOrFriends, Props.affectsEnemies,
                prcDebug
            );

            foreach (Pawn curPawn in affectedPawnList)
            {
                // Slugs are immune to this
                if (curPawn.IsSlug())
                {
                    Tools.Warn(curPawn.Label + " is slug, not affected", prcDebug);
                    continue;
                }
                
                // Add psychicSensitivity * SocialImpact * SocialSkill fight here
                if (ToolsHediff.ApplyHediffOnBodyPartTag(curPawn, BodyPartTagDefOf.ConsciousnessSource, hediffDefToApply, prcDebug))
                {
                    if (IsFlaySpot)
                    {
                        Thought_Memory MindFlayed = (Thought_Memory)ThoughtMaker.MakeThought(MyDefs.MindFlayThought);
                        curPawn.needs.mood.thoughts.memories.TryGainMemory(MindFlayed, Initiator);
                    }

                    //GfxEffects.ThrowPsycastAreaMote(curPawn.Position.ToVector3(), myMap);
                    GfxEffects.ThrowMindMote(curPawn.Position.ToVector3(), myMap, spotKind);
                    AffectedPawnsNum++;
                }
            }

            if (IsLimitExceeded)
            {
                Initiator.ApplyTiredness();
            }
                

            //Checking if Initiator is not mad or downed or sleepin or on fire
            if (Initiator.InMentalState || Initiator.Downed || Initiator.IsSleepingOrOnFire() || AffectedPawnsNum > Props.hediffAppliedLimit)
            {
                GfxEffects.ThrowCoupleMotes(buildingPos, myMap, spotKind);
                building.Destroy();
            }
                
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            /*
             * spotkind etc maybe
             */
            Scribe_References.Look(ref Initiator, "Initiator");
            Scribe_Values.Look(ref AffectedPawnsNum, "FlayageNum");
        }

        public override string CompInspectStringExtra()
        {
            string text = base.CompInspectStringExtra();
            string result = string.Empty;

            if(IsFlaySpot)
                result += MyDefs.FlayCompInspectStringExtra;
            else if (IsFondleSpot)
                result += MyDefs.FondleCompInspectStringExtra;
            else
            {
                Tools.Warn("CompInspectStringExtra - spotkind weird", myDebug);
            }
            result += AffectedPawnsNum;


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
                    defaultDesc = "process debug\n",
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
                // dump props
                yield return new Command_Action
                {
                    defaultLabel = "dumpProps",
                    defaultDesc = DumpProps,
                    action = delegate
                    {
                        Tools.Warn(DumpProps, true);
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
            if(IsFlaySpot)
                GenDraw.DrawLineBetween(parent.TrueCenter(), Initiator.TrueCenter(), SimpleColor.Magenta);
            else if(IsFondleSpot)
                GenDraw.DrawLineBetween(parent.TrueCenter(), Initiator.TrueCenter(), SimpleColor.Green);

            if (Range > 0f)
            {
                // Cannot draw radius ring of radius 140.7: not enough squares in the precalculated list.
                if (Range < GenRadial.MaxRadialPatternRadius)
                    GenDraw.DrawRadiusRing(this.parent.Position, Range);
            }
        }
    }
}
