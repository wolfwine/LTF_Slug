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
            if (vestiShell == null)
            {
                Tools.Warn(pawn.LabelShort + " has no vestigial shell", myDebug);
                return false;
            }
            return true;
        }

    }
}
