using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_NaturalSlugDewExudator : HediffGiver
    {
        private readonly string ErrStr = "HediffGiver_NaturalSlugDewExudator denied bc ";
        //private readonly bool myDebug = true;
        private readonly bool myDebug = false;

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())
            {
                //if (pawn.Spawned)                    Tools.Warn(ErrStr + pawn.LabelShort + " is  not slug", myDebug);
                return false;
            }


            if (pawn.Spawned && hediff.Part != pawn.GetStomach())
            {
                if (pawn.Spawned) Tools.Warn(pawn.LabelShort + "'s"+ErrStr + "hediff.Part(" + hediff?.Part?.def?.defName + ") != pawn.GetStomach()", myDebug);
                return false;
            }

            /*
            if (pawn.HasNaturalSlugDewExudator())
            {
                //Tools.Warn(ErrStr + pawn.LabelShort + " pawn.HasNaturalSlugDewExudator()", myDebug);
                return false;
            }
            */


            bool appliedHediff = TryApply(pawn, null);
            if (appliedHediff)
            {
                if (pawn.Spawned) Tools.Warn(pawn.LabelShort + "'s HediffGiver_SlugTrail applied " + this.hediff.defName);
                return true;
            }

            return false;
        }

    }
}
