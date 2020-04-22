using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_SlugTrail : HediffGiver
    {
        public bool AlwaysRainbow = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().AlwaysRainbowPuddle;
        public bool EnablePuddles = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnablePuddles;

        public bool myDebug = true;

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())return false;

            if (!EnablePuddles)
                return false;

            if (AlwaysRainbow)
                this.hediff = MyDefs.RainbowTrailHediff;


            bool appliedHediff = TryApply(pawn, null);
            if (appliedHediff)
                return true;

            return false;
        }
    
    }
}
