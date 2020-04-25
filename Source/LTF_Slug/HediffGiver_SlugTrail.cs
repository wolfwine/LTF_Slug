using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffGiver_SlugTrail : HediffGiver
    {
        private bool AlwaysRainbow = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().AlwaysRainbowPuddle;
        private bool EnablePuddles = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnablePuddles;

        //private readonly bool myDebug = true;
        private readonly bool myDebug = false;
        private readonly string ErrStr = "HediffGiver_SlugTrail denied bc ";

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())
            {
                if(pawn.Spawned)
                    Tools.Warn(pawn.LabelShort + "'s" + ErrStr + " is not slug", myDebug);
                return false;
            }

            if (!EnablePuddles)
            {
                if (pawn.Spawned)
                    Tools.Warn(ErrStr + " EnablePuddles = "+EnablePuddles, myDebug);
                return false;
            }
                
            if (AlwaysRainbow)
            {
                if (pawn.Spawned)
                    Tools.Warn(pawn?.LabelShort + "'s" + " HediffGiver_SlugTrail this.hediff = MyDefs.RainbowTrailHediff; AlwaysRainbow = " + AlwaysRainbow, myDebug);
                this.hediff = MyDefs.RainbowTrailHediff;
            }
            
            if (pawn.Spawned && hediff.Part != pawn.GetgrooveSole())
            {
                if (pawn.Spawned)
                    Tools.Warn(pawn?.LabelShort + "'s" + ErrStr + "hediff.Part(" + hediff?.Part?.def?.defName + ") != pawn.GetgrooveSole()", myDebug);
                return false;
            }

            /*
            if (pawn.HasNaturalSlugTrail())
            {
                //Tools.Warn(ErrStr + pawn.LabelShort + " pawn.HasNaturalSlugTrail()", myDebug);
                return false;
            }
            */    

            bool appliedHediff = TryApply(pawn, null);
            if (appliedHediff)
            {
                if (pawn.Spawned)
                    Tools.Warn(pawn?.LabelShort + "'s HediffGiver_SlugTrail applied " + this.hediff?.defName, myDebug);
                return true;
            }

            return false;
        }
    
    }
}
