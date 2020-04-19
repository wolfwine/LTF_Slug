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
    public class ToolsPawn
    {
        private static string vestiShellName = "VestigialShellBP";

        public static bool CheckPawn(Pawn pawn)
        {
            //return (pawn != null && pawn.Map != null && pawn.Position != null);
            return (pawn != null && pawn.Map != null);
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

        public static BodyPartRecord GetVestiShell(Pawn pawn)
        {
            BodyPartDef vestiShell = DefDatabase<BodyPartDef>.AllDefs.Where((BodyPartDef b) => b.defName == vestiShellName).RandomElement();

            pawn.RaceProps.body.GetPartsWithDef(vestiShell).TryRandomElement(out BodyPartRecord bodyPart);
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

            Hediff hediff = HediffMaker.MakeHediff(MyXmlDef.MindFlayedHediff, pawn, bodyPart);
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
