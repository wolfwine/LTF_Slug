using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_SlugTrail : HediffGiver
    {
        public bool AlwaysRainbow = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().AlwaysRainbowPuddle;
        public bool NoPuddle = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().NoPuddle;

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())
                return false;
            if (NoPuddle)
                return false;

            if (AlwaysRainbow)
                this.hediff = MyDefs.RainbowTrailHediff;

            TryApply(pawn, null);
            /*
            bool appliedHediff = AlwaysRainbow ? HediffGiverUtility.TryApply(pawn, MyDefs.RainbowTrailHediff, partsToAffect) : TryApply(pawn, null);
            if (appliedHediff)
            {
                pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def).Severity = .1f;
            }
            */

            return false;
        }
    }
}
