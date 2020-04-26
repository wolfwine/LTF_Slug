/*
 * Created by SharpDevelop.
 * User: Etienne
 * Date: 22/11/2017
 * Time: 16:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
    public class HediffComp_WaitUntilBPToReset : HediffComp
    {
        int ticksLeftBeforeNextCheck=0;

        Pawn pawn;
        string pLabel;

        public HeDiffCompProperties_WaitUntilBPToReset Props
        {
            get
            {
                return (HeDiffCompProperties_WaitUntilBPToReset)this.props;
            }
        }

        public override void CompPostMake()
        {
            //base.CompPostMake();
            pawn = parent.pawn;
            pLabel = pawn.Label;

            SetTicks();

            Tools.Warn(pLabel + " Entering HediffComp_WaitUntilBPToReset.CompPostMake", Props.debug);
        }

        /*
        public override void CompExposeData()
        {
            base.CompExposeData();

            Scribe_Values.Look(ref ticksLeftBeforeNextCheck, "ticksLeftBeforeNextCheck");
        }
        */

        private void SetTicks()
        {
            ticksLeftBeforeNextCheck = Props.period + Rand.Range(0, Props.period);
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Tools.CheckPawn(pawn))
            {
                parent.Severity = 0;
                return;
            }

            //Tools.Warn(pLabel + " entering HeDiffComp_ResetAbilities.CompPostTick - Ticks to wait: "+ticksLeftBeforeNextCheck, myDebug);

            if (ticksLeftBeforeNextCheck > 0)
            {
                //Tools.Warn("waiting ..."+ ticksLeftBeforeNextCheck + " ticks left", myDebug);
                ticksLeftBeforeNextCheck--;
                return;
            }

            if (pawn.HasNaturalVestigialShell())
            {
                Tools.Warn("Trying tp reset abilities then die", Props.debug);
                ToolsAbilities.AbilitiesReset(pawn, Props.debug);
                parent.Severity = 0;
            }
            else
                SetTicks();

        }

        public override string CompTipStringExtra
        {
            get
            {
                string result = string.Empty;

                result += "Waiting until vestigal shell is there again to disappear; " + ticksLeftBeforeNextCheck + " ticks left before next check";

                return result;
            }
        }
    }
}
