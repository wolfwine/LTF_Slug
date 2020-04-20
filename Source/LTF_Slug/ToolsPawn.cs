using RimWorld;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;


namespace LTF_Slug
{
    public static class ToolsPawn
    {
        public static bool CheckPawn(Pawn pawn)
        {
            return (pawn != null && pawn.Map != null);
        }

        public static bool IsSlug(this Pawn pawn)
        {
            return (pawn?.def.defName == MyDefs.slugDefName);
        }

        public static bool IsSleepingOrOnFire(this Pawn pawn)
        {
            if (pawn.jobs.curJob != null && !pawn.jobs.IsCurrentJobPlayerInterruptible())
            {
                return true;
            }
            return false;
        }

        private static BodyPartRecord GetHeart(Pawn pawn, bool myDebug = false)
        {
            BodyPartRecord bodyPart = null;
            pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BloodPumpingSource).TryRandomElement(out bodyPart);
            if (bodyPart == null)
            {
                Tools.Warn("null heart ?", myDebug);
            }

            return bodyPart;
        }
        public static BodyPartRecord GetBrain(Pawn pawn)
        {
            pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }
        public static BodyPartRecord GetStomach(Pawn pawn)
        {
            pawn.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Stomach).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }

        public static BodyPartRecord GetBPRecord(this Pawn pawn, string BPPartDefName)
        {
            BodyPartDef vestiShell = DefDatabase<BodyPartDef>.AllDefs.Where((BodyPartDef b) => b.defName == BPPartDefName).RandomElement();
            pawn.RaceProps.body.GetPartsWithDef(vestiShell).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }
        public static BodyPartRecord GetVestigialShell(this Pawn pawn)
        {
            return GetBPRecord(pawn, MyDefs.vestigialShellName);
        }
        public static BodyPartRecord GetFondlingVestigialShell(this Pawn pawn)
        {
            return GetBPRecord(pawn, MyDefs.fondlingVestigialShellName);
        }
        public static bool HasNaturalVestigialShell(this Pawn pawn, bool myDebug=false)
        {
            BodyPartRecord vestiShell = pawn.GetVestigialShell();
            if (vestiShell == null)
            {
                Tools.Warn(pawn.LabelShort + " has no vestigial shell", myDebug);
                return false;
            }
            return true;
        }
        public static bool HasFondlingVestigialShell(this Pawn pawn, bool myDebug = false)
        {
            BodyPartRecord vestiShell = pawn.GetFondlingVestigialShell();
            if (vestiShell == null)
            {
                Tools.Warn(pawn.LabelShort + " has no fondling vestigial shell", myDebug);
                return false;
            }
            return true;
        }

        public static bool ApplyHediffOnBodyPartTag(Pawn pawn, BodyPartTagDef BPTag, HediffDef hediffDef, bool myDebug)
        {
            pawn.RaceProps.body.GetPartsWithTag(BPTag).TryRandomElement(out BodyPartRecord bodyPart);
            if (bodyPart == null)
            {
                Tools.Warn("null body part", myDebug);
                return false;
            }

            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPart);
            if (hediff == null)
            {
                Tools.Warn("hediff maker null", myDebug);
                return false;
            }

            pawn.health.AddHediff(hediff, bodyPart, null);

            return true;
        }

        // -10% rest
        public static void ApplyTiredness(this Pawn pawn)
        {
            pawn.needs.rest.CurLevel = pawn.needs.rest.CurLevel * .9f;
        }
    }
}
