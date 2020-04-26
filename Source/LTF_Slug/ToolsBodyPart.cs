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
    public static class ToolsBodyPart
    {
 
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
        public static BodyPartRecord GetStomach(this Pawn pawn)
        {
            pawn.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Stomach).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }
        public static BodyPartRecord GetBPRecord(this Pawn pawn, string BPPartDefName, bool myDebug = false)
        {
            IEnumerable<BodyPartDef> BPDefIE = DefDatabase<BodyPartDef>.AllDefs.Where((BodyPartDef b) => b.defName == BPPartDefName);
            if (BPDefIE.EnumerableNullOrEmpty())
            {
                Tools.Warn(pawn.Label+" - GetBPRecord - did not find any " + BPPartDefName, myDebug);
                return null;
            }
                
            BodyPartDef BPDef = BPDefIE.RandomElement();
            pawn.RaceProps.body.GetPartsWithDef(BPDef).TryRandomElement(out BodyPartRecord bodyPart);

            Tools.Warn(pawn.Label + "GetBPRecord - DID find " + BPPartDefName, myDebug);
            return bodyPart;
        }
        public static BodyPartRecord GetgrooveSole(this Pawn pawn, bool myDebug = false)
        {
            return GetBPRecord(pawn, MyDefs.grooveSoleName, myDebug);
        }
        public static BodyPartRecord GetVestigialShell(this Pawn pawn, bool myDebug = false)
        {
            return GetBPRecord(pawn, MyDefs.vestigialShellName, myDebug);
        }

        public static bool HasNaturalVestigialShell(this Pawn pawn, bool myDebug=false)
        {
            BodyPartRecord vestiShell = pawn.GetVestigialShell();
            if ((vestiShell == null) || pawn.health.hediffSet.HasHediff(HediffDefOf.MissingBodyPart, vestiShell))
            {
                Tools.Warn(pawn.LabelShort + " has no vestigial shell", myDebug);
                return false;
            }
            return true;
        }

        public static bool HasFondlingVestigialShell(this Pawn pawn, bool myDebug = false)
        {
            BodyPartRecord vestiShell = pawn.GetVestigialShell(myDebug);
            if (vestiShell == null)
            {
                Tools.Warn(pawn.LabelShort + " has a vestigial shell, and may have a fondling hediff", myDebug);
                return false;
            }

            //public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false);
            return pawn.health.hediffSet.HasHediff(MyDefs.MindFondlingHediff, vestiShell);
        }

        // Check if pawn has slug natural hediffs aka part from HediffGiverSetDef or from BS
        //HediffGiver_SlugTrail_List HediffGiver_NaturalSlugDewExudator_List
        public static bool HasNaturalSlugTrail(this Pawn pawn, bool myDebug = false)
        {
            BodyPartRecord grooveSole = pawn.GetgrooveSole(myDebug);
            if (grooveSole == null)
            {
                Tools.Warn(pawn.LabelShort + " has a grooveSole, and may HasNaturalSlugTrail", myDebug);
                return false;
            }

            foreach (HediffDef curHD in MyDefs.HediffGiver_SlugTrail_List)
            {
                if (pawn.health.hediffSet.HasHediff(curHD, grooveSole))
                    return true;
            }

            return false;
        }
        public static bool HasNaturalSlugDewExudator(this Pawn pawn, bool myDebug = false)
        {
            BodyPartRecord stomach = pawn.GetStomach();
            if (stomach == null)
            {
                Tools.Warn(pawn.LabelShort + " has a stomach, and may HasNaturalSlugDewExudator", myDebug);
                return false;
            }

            foreach (HediffDef curHD in MyDefs.HediffGiver_NaturalSlugDewExudator_List)
            {
                if (pawn.health.hediffSet.HasHediff(curHD, stomach))
                    return true;
            }

            return false;
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

        public static bool AddWaitingForVestigal(Pawn pawn, bool myDebug)
        {
            return ApplyHediffOnBodyPartTag(pawn, BodyPartTagDefOf.ConsciousnessSource, MyDefs.WaitingForVestigial, myDebug);
        }

    }
}
