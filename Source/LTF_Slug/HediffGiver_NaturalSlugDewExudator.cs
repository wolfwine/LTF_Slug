using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_NaturalSlugDewExudator : HediffGiver
    {

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())
                return false;

            bool appliedHediff = TryApply(pawn, null);

            if (appliedHediff)
                return true;

            return false;
        }
    }
}
