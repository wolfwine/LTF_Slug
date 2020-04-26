using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using AbilityUser;

namespace LTF_Slug
{
    public class HediffGiver_VestigialShellAbility : HediffGiver
    {

        //private readonly bool myDebug = true;
        private readonly bool myDebug = true;
        /*
        private readonly string myName = "HediffGiver_VestigialShellAbility";
        private readonly string ErrStr = "HediffGiver_VestigialShellAbility denied bc ";
        */
        
        private string pLabel;

        public bool IsFlayer(Pawn pawn)
        {
            CompMindFlayer compMindFlayer = pawn.TryGetComp<CompMindFlayer>();
            if (compMindFlayer != null)
                return compMindFlayer.IsMindFlayer;
            
            return false;
        }
        public bool IsFondler(Pawn pawn)
        {
            CompMindFondler compMindFondler = pawn.TryGetComp<CompMindFondler>();
            if (compMindFondler != null)
                return compMindFondler.IsMindFondler;

            return false;
        }

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.Spawned)
                return false;

            pLabel = pawn?.LabelShort;

            if (!pawn.IsSlug())
            {
                Tools.Warn(pLabel + " is not slug - false", myDebug);
                return false;
            }

            BodyPartRecord pawnBPR = pawn.GetVestigialShell();
            Tools.Warn(">> Entering " + pLabel + "'s vestigial HediffGiver;", myDebug);
            Tools.Warn(" hediff.def.defName: " + hediff?.def?.defName + "; this.hediff.def.defname: " + this.hediff?.defName, myDebug);
            Tools.Warn(" hediff.Part: " + hediff?.Part?.def?.defName + "; pawnBPR:" + pawnBPR.def.defName, myDebug);

            /*
            if (hediff.Part != pawnBPR)
            {
                Tools.Warn(pLabel + " h.part=" + hediff.Part.def.defName + " != " + pawnBPR.def.defName + " - false", myDebug);
                return false;
            }
            */

            if(hediff.def == HediffDefOf.Anesthetic && hediff.Part == null)
            {
                Tools.Warn(pLabel + " had Anesthetic applied on whole body", myDebug);
                return false;
            }

            /*
            bool appliedHediff = TryApply(pawn, null);
            if (appliedHediff)
            {
                Tools.Warn(pLabel + " had " + this.hediff?.defName + " applied ", myDebug);



                return true;
            }
            */

            if (hediff.def == MyDefs.MindFondlingHediff)
            {
                ToolsAbilities.FondlerReset(pawn, myDebug);
                Tools.Warn(pLabel + " called ResetAbilities bc " + hediff.def.defName, myDebug);

                return false;
            }
            else if (hediff.def == HediffDefOf.MissingBodyPart &&
                hediff.Part.def.defName == MyDefs.vestigialShellName &&
                !ToolsBodyPart.HasNaturalVestigialShell(pawn, myDebug))
            {
                ToolsAbilities.AbilitiesReset(pawn, myDebug);
                Tools.Warn(pLabel + " called ResetAbilities bc " + hediff.def.defName, myDebug);

                ToolsBodyPart.AddWaitingForVestigal(pawn, myDebug);
                Tools.Warn(pLabel + " added WaitingForVestigal", myDebug);

                return false;
            }

            Tools.Warn(hediff.def.defName + " not applied on " + pLabel, myDebug);

            return false;
        }
    
    }
}
