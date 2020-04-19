using RimWorld;
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
        
        public static Thing CreateMindFondleSpot(IntVec3 destinationCell, Map map)
        {
            Building mindFlaySpot = (Building)ThingMaker.MakeThing(ThingDef.Named("LTF_MindFondleSpot"), null);

            //lockBlock.SetColor(lockDowner.DrawColor);
            GenSpawn.Spawn(mindFlaySpot, destinationCell, map, Rot4.North, WipeMode.Vanish);

            mindFlaySpot.SetFaction(Faction.OfPlayer);

            int randMotesNum = Rand.Range(3, 7);

            for(int i=0;i<randMotesNum;i++)
                GfxEffects.ThrowMindFondleMote(destinationCell.ToVector3(), map);
            
            foreach (IntVec3 puff in GenAdj.CellsAdjacent8Way(mindFlaySpot))
                if(puff.InBounds(map))
                    MoteMaker.ThrowAirPuffUp(puff.ToVector3(), map);

            return (mindFlaySpot);
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
              /*  
            Thing myNewSpot = CreateMindFondleSpot(correctedCell, CasterPawn.Map);
            if(myNewSpot == null)
            {
                Tools.Warn("myNewSpot is null after CreateMindFondleSpot", myDebug);
                return;
            }
            */
            /*
            Comp_LTF_MindFondleSpot MindFondleSpotComp = myNewSpot.TryGetComp<Comp_LTF_MindFondleSpot>();
            if(MindFondleSpotComp == null) { 
                Tools.Warn("MindFondleSpotComp is null after TryGetComp", myDebug);
                return;
            }

            MindFondleSpotComp.SetPawn(CasterPawn);
            */
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