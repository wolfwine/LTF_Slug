using RimWorld;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;


namespace LTF_Slug
{
    public static class ToolsAbilities
    {
        public static void FlayerReset(Pawn pawn, bool debug = false)
        {
            CompMindFlayer compMindFlayer = pawn.TryGetComp<CompMindFlayer>();
            if (compMindFlayer != null)
            {
                Tools.Warn("Reseting Flayer", debug);

                compMindFlayer.RemovePawnAbility(MindFlayerDefOf.LTF_Slug_MindFlayer);
                compMindFlayer.MindFlayer = null;
            }
            else
                Tools.Warn("cannot find Flayer ability", debug);
        }

        public static void FondlerReset(Pawn pawn, bool debug = false)
        {
            CompMindFondler compMindFondler = pawn.TryGetComp<CompMindFondler>();
            if (compMindFondler != null)
            {
                Tools.Warn("Reseting Fondler", debug);

                compMindFondler.RemovePawnAbility(MindFondlerDefOf.LTF_Slug_MindFondler);
                compMindFondler.MindFondler = null;
            }
            else
                Tools.Warn("cannot find MindFondler ability", debug);
        }

        public static void AbilitiesReset(Pawn pawn, bool debug = false)
        {
            //foreach(AbilityUser.AbilityDef abilityDef in Props.abilitiesToReset){};
            Tools.Warn("Entering ToolsAbilities.ResetAbilities", debug);

            FlayerReset(pawn, debug);
            FondlerReset(pawn, debug);
        }

        public static IEnumerable<Gizmo> GetAbilityReportGizmo(AbilityUser.AbilityData abilityData)
        {
            if (Prefs.DevMode)
            {
                string powerString = string.Empty;
                for (int i = 0; i < abilityData.AllPowers.Count; i++)
                    powerString += i + ":" + abilityData.AllPowers[i].Def.defName + "; ";

                yield return new Command_Action
                {
                    defaultLabel = "power Num",
                    defaultDesc = "n=" + abilityData.AllPowers.Count + ";\n" + powerString
                };
            }
        }
        public static IEnumerable<Gizmo> GetAbilityGizmos(AbilityUser.AbilityData abilityData)
        {
            for (int i = 0; i < abilityData.AllPowers.Count; i++)
            {
                AbilityUser.PawnAbility myAbility = abilityData.AllPowers[i];
                yield return myAbility.GetGizmo();

                if (Prefs.DevMode)
                    yield return new Command_Action
                    {
                        defaultLabel = "reset " + myAbility.Def.label + " cooldown",
                        defaultDesc = "cooldown=" + myAbility.CooldownTicksLeft,
                        action = delegate
                        {
                            myAbility.CooldownTicksLeft = -1;
                        }
                    };
            }
        }
    }
}
