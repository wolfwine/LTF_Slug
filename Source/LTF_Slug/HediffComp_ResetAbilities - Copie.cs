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
    public class HeDiffComp_ResetAbilities : HediffComp
    {
        bool myDebug = false;

        int waitingTicks;
        bool didIt = false;
        Pawn pawn;
        string pLabel;

        public HeDiffCompProperties_ResetAbilities Props
        {
            get
            {
                return (HeDiffCompProperties_ResetAbilities)this.props;
            }
        }

        public override void CompPostMake()
        {
            //base.CompPostMake();
            pawn = parent.pawn;
            pLabel = pawn.Label;

            myDebug = Props.debug;
            waitingTicks = Props.waitingTicks;

            Tools.Warn(pLabel + "Entering HeDiffComp_ResetAbilities.CompPostMake debug:" + myDebug, myDebug);
        }
        /*
        public bool HasAbilitiesToReset
        {
            get
            {
                return !Props.abilitiesToReset.NullOrEmpty();
            }
        }
        */
        public void ResetAbilities(Pawn pawn)
        {
            //foreach(AbilityUser.AbilityDef abilityDef in Props.abilitiesToReset){};
            Tools.Warn("Entering HeDiffComp_ResetAbilities.ResetAbilities", myDebug);
            CompMindFlayer compMindFlayer = pawn.TryGetComp<CompMindFlayer>();
            if (compMindFlayer != null)
            {
                Tools.Warn("Reseting " + pLabel + ".compMindFlayer", myDebug);
                compMindFlayer.MindFlayer = null;
                didIt = true;
            }
            else
                Tools.Warn("cannot find" + pLabel + " MindFondler ability", myDebug);

            CompMindFondler compMindFondler = pawn.TryGetComp<CompMindFondler>();
            if (compMindFondler != null)
            {
                Tools.Warn("Reseting " + pLabel + " MindFondler", myDebug);
                compMindFondler.MindFondler = null;
                didIt = true;
            }
            else
                Tools.Warn("cannot find " + pLabel + " MindFondler ability", myDebug);

        }
        
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Tools.CheckPawn(pawn))
            {
                parent.Severity = 0;
                return;
            }

            Tools.Warn(pLabel + " entering HeDiffComp_ResetAbilities.CompPostTick - Ticks to wait: "+waitingTicks, myDebug);

            if (waitingTicks > 0)
            {
                Tools.Warn("waiting ..."+ waitingTicks+" ticks left", myDebug);
                waitingTicks--;
                return;
            }

            Tools.Warn("Trying tp reset abililities", myDebug);
            ResetAbilities(pawn);

            // suicide
            if (didIt)
            {
                Tools.Warn("Did it, killing reset abilities hediff", myDebug);
                parent.Severity = 0;
            }
                
        }

        public override string CompTipStringExtra
        {
            get
            {
                string result = string.Empty;
                
                result += "ResetAbilities - This should disappear very fast";
                
                return result;
            }
        }
    }
}
