﻿using RimWorld;
using AlienRace;
using Verse;
using Verse.AI;
using UnityEngine;
using System.Collections.Generic;
using AbilityUser;

using System;


namespace LTF_Slug
{

    public class MindFondleEffect : AbilityUser.Verb_UseAbility
    {
        private const bool myDebug = false;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            //Tools.Warn("CanHitTargetFrom : Blink in TryCastShot with root" + root + " and target" + targ, myDebug);
            if (UseAbilityProps.AbilityTargetCategory == AbilityTargetCategory.TargetLocation)
            {
                //Tools.Warn("AbilityTargetCategory == TargetLocation; base.CanHitTargetFrom", myDebug);
                return base.CanHitTargetFrom(root, targ);
            }
            //Tools.Warn("??? AbilityTargetCategory != TargetLocation; TryFindShootLineFromTo", myDebug);
            return TryFindShootLineFromTo(root, targ, out ShootLine shootLine);
        }

        protected override bool TryCastShot()
        {
            //Tools.Warn("Entering TryCastShot", myDebug);

            if (currentTarget != null && CasterPawn != null && currentTarget.Cell != null && currentTarget.Cell.IsValid)
            {
                base.TryCastShot();
                return true;
            }
            else
            {
                Tools.Warn("failed to TryCastShot", myDebug);
            }

            // Setting cooldown
            //ability.TicksUntilCasting = (int)this.UseAbilityProps.SecondsToRecharge * GenTicks.TicksPerRealSecond;

            return false;
        }

        public void Effect()
        {
            IntVec3 correctedCell = currentTarget.Cell;

            if (ToolsCell.CellHasBuildingOrMaturePlant(currentTarget.Cell, CasterPawn.Map, myDebug))
                correctedCell = ToolsCell.GetCloserCell(CasterPawn.Position, currentTarget.Cell, CasterPawn.Map, myDebug);

            if (correctedCell == IntVec3.Zero)
            {
                Tools.Warn("Failed to find a better cell", myDebug);
                return;
            }

            Thing myNewSpot = MindSpotUtils.CreateMindSpot(correctedCell, CasterPawn.Map, MyDefs.SpotKind.fondle);
            if (myNewSpot == null)
            {
                Tools.Warn("myNewSpot is null after CreateMindFlaySpot", myDebug);
                return;
            }

            MindSpotUtils.TryGetMindSpotComp(myNewSpot).SetPawn(CasterPawn);
        }

        // Necessary for autocomplete ability
        public override void PostCastShot(bool inResult, out bool outResult)
        {
            if (inResult)
            {
                Effect();
                outResult = true;
            }
            outResult = inResult;
        }
    }
}