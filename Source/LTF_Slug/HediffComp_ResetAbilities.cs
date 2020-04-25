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
        const int tickLimiterModulo = 60;
        bool myDebug = false;
        bool blockAction = false;
        int waitingTicks;
        bool didIt = false;

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
            Tools.Warn("Entering HeDiffComp_ResetAbilities.CompPostMake", myDebug);
            myDebug = Props.debug;
            waitingTicks = Props.waitingTicks;
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
            Tools.Warn("Entering ResetAbilities", myDebug);
            CompMindFlayer compMindFlayer = pawn.TryGetComp<CompMindFlayer>();
            if (compMindFlayer != null)
            {
                Tools.Warn("Reseting " + pawn.Label + ".compMindFlayer", myDebug);
                compMindFlayer.MindFlayer = null;
                didIt = true;
            }
            else
                Tools.Warn("cannot find" + pawn.Label + " MindFondler ability", myDebug);

            CompMindFondler compMindFondler = pawn.TryGetComp<CompMindFondler>();
            if (compMindFondler != null)
            {
                Tools.Warn("Reseting " + pawn.Label + " MindFondler", myDebug);
                compMindFondler.MindFondler = null;
                didIt = true;
            }
            else
                Tools.Warn("cannot find " + pawn.Label + " MindFondler ability", myDebug);

        }
        
        public override void CompPostTick(ref float severityAdjustment)
        {
            Pawn pawn = parent.pawn;
            if (!Tools.CheckPawn(pawn))
            {
                parent.Severity = 0;
                return;
            }

            Tools.Warn(pawn.LabelShort+" entering HeDiffComp_ResetAbilities.CompPostTick - Ticks to wait: "+waitingTicks, myDebug);

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
