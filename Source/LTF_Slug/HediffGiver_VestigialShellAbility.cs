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
        private readonly string myName = "HediffGiver_VestigialShellAbility";
        private readonly string ErrStr = "HediffGiver_VestigialShellAbility denied bc ";
        private bool DidChangeAbilities = false;

        /*
        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            Tools.Warn(pawn.Label + " Entering OnIntervalPassed", myDebug);

            if (!pawn.Spawned)
                return;
            bool FlayDesync =   (pawn.HasNaturalVestigialShell() != IsFlayer(pawn));
            bool FondleDesync = (pawn.HasFondlingVestigialShell() != IsFondler(pawn));

            Tools.Warn(pawn.Label + " Flay desync: HasNatural: "+ pawn.HasNaturalVestigialShell()+"; IsFlayer:"+IsFlayer(pawn), FlayDesync && myDebug);
            Tools.Warn(pawn.Label + " Fondle desync; HasFondling: "+ pawn.HasFondlingVestigialShell()+"; IsFondler: "+IsFondler(pawn), FondleDesync && myDebug);
            if ( FlayDesync || FondleDesync )
                ResetAbilities(pawn);
        }
        */
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

        public void ResetAbilities(Pawn pawn)
        {
            DidChangeAbilities = false;

            //foreach(AbilityUser.AbilityDef abilityDef in Props.abilitiesToReset){};
            Tools.Warn(pawn.Label + " Entering HediffGiver_VestigialShellAbility ResetAbilities", myDebug);
            CompMindFlayer compMindFlayer = pawn.TryGetComp<CompMindFlayer>();
            if (compMindFlayer != null)
            {
                Tools.Warn("Reseting " + pawn.Label + ".compMindFlayer", myDebug);
                //compMindFlayer.MindFlayer = null;
                compMindFlayer.Initialize();
                DidChangeAbilities = true;
            }
            else
                Tools.Warn("cannot find" + pawn.Label + " MindFondler ability", myDebug);

            CompMindFondler compMindFondler = pawn.TryGetComp<CompMindFondler>();
            if (compMindFondler != null)
            {
                Tools.Warn("Reseting " + pawn.Label + " MindFondler", myDebug);
                //compMindFondler.MindFondler = null;
                compMindFondler.Initialize();
                DidChangeAbilities = true;
            }
            else
                Tools.Warn("cannot find " + pawn.Label + " MindFondler ability", myDebug);

            Tools.Warn(pawn.Label + " Exiting HediffGiver_VestigialShellAbility ResetAbilities - DidIt: "+DidChangeAbilities, myDebug);

        }

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!pawn.IsSlug())
            {
                if(pawn.Spawned)
                    Tools.Warn(pawn.LabelShort + "'s" + ErrStr + " is not slug", myDebug);
                return false;
            }

            if (pawn.Spawned && hediff.Part != pawn.GetVestigialShell() )
            {
                if (pawn.Spawned)
                    Tools.Warn(pawn?.LabelShort + "'s" + ErrStr + "hediff.Part(" + hediff?.Part?.def?.defName + ") != pawn.GetVestigialShell()", myDebug);
                return false;
            }

            bool appliedHediff = TryApply(pawn, null);
            if (appliedHediff)
            {
                if (pawn.Spawned)
                    Tools.Warn(pawn?.LabelShort + "'s HediffGiver_VestigialShellAbility applied " + this.hediff?.defName, myDebug);

                Tools.Warn(pawn.Label + "'s HediffGiver_VestigialShellAbility OnHediffAdded calling ResetAbilities", myDebug);
                ResetAbilities(pawn);
                return true;
            }

            return false;
        }
    
    }
}
