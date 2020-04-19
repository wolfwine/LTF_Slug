using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_NaturalSlugDewExudator : HediffGiver
    {
        public string letterLabel;
        public string letter;

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            bool validPawn = (pawn?.def.defName == "Alien_Slug");
            if (!validPawn)
                return false;


            bool bla = pawn.IsColonist;
            bool appliedHediff = base.TryApply(pawn, null);
            if (hediff.Part != ToolsPawn.GetStomach(pawn))
            {
                return false;
            }
            if (appliedHediff)
            {
                if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
                {
                    Find.LetterStack.ReceiveLetter(
                        this.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN"), 
                        this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN"), 
                        LetterDefOf.NegativeEvent, pawn, null, null);
                }
                return true;
            }

            return false;
        }
    }
}
