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
        private static readonly string vestiShellName = "VestigialShellBP";
        private static readonly string slugDefName = "Alien_Slug";

        public static bool CheckPawn(Pawn pawn)
        {
            //return (pawn != null && pawn.Map != null && pawn.Position != null);
            return (pawn != null && pawn.Map != null);
        }

        /*
        public static bool IsSlug{
            get
            {
                return (this.pawn.IsSlug());
            }
        }
        */
        public static bool IsSlug(this Pawn pawn)
        {
            return (pawn?.def.defName == slugDefName);
        }

        public static bool IsSleepingOrOnFire(this Pawn pawn)
        {
            if (pawn.jobs.curJob != null && !pawn.jobs.IsCurrentJobPlayerInterruptible())
            {
                return true;
            }
            return false;
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

        public static BodyPartRecord GetVestiShell(this Pawn pawn)
        {
            BodyPartDef vestiShell = DefDatabase<BodyPartDef>.AllDefs.Where((BodyPartDef b) => b.defName == vestiShellName).RandomElement();

            pawn.RaceProps.body.GetPartsWithDef(vestiShell).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }

        private static BodyPartRecord GetHeart(Pawn pawn, bool myDebug=false)
        {
            BodyPartRecord bodyPart = null;
            //pawn.RaceProps.body.GetPartsWithTag("BloodPumpingSource").TryRandomElement(out bodyPart);
            pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BloodPumpingSource).TryRandomElement(out bodyPart);
            if (bodyPart == null)
            {
                Tools.Warn("null heart ?", myDebug);
            }

            return bodyPart;
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
    }
}
