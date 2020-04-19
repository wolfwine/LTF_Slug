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

    public class MindFlayEffect : AbilityUser.Verb_UseAbility
    {
        private const bool myDebug = true;

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
                //                Tools.Warn("Valid target in TryCastShot", myDebug);
                //CreateMindFlaySpot(currentTarget.Cell, CasterPawn.Map);
                base.TryCastShot();
                //                Tools.Warn("created MindFlaySpot in TryCastShot", myDebug);
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
        
        public static Thing CreateMindFlaySpot(IntVec3 destinationCell, Map map)
        {
            Building mindFlaySpot = (Building)ThingMaker.MakeThing(ThingDef.Named("LTF_MindFlaySpot"), null);

            //lockBlock.SetColor(lockDowner.DrawColor);
            GenSpawn.Spawn(mindFlaySpot, destinationCell, map, Rot4.North, WipeMode.Vanish);

            mindFlaySpot.SetFaction(Faction.OfPlayer);

            int randMotesNum = Rand.Range(3, 7);

            for(int i=0;i<randMotesNum;i++)
                ThrowMicroFlakes(destinationCell.ToVector3(), map);
            
            foreach (IntVec3 puff in GenAdj.CellsAdjacent8Way(mindFlaySpot))
                if(puff.InBounds(map))
                    MoteMaker.ThrowAirPuffUp(puff.ToVector3(), map);

            return (mindFlaySpot);
        }

        public static void ThrowMicroFlakes(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("Mote_MindFlay"), null);
            moteThrown.Scale = Rand.Range(0.5f, 1.2f);

            moteThrown.rotationRate = Rand.Range(0f, 359f);

            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);

            moteThrown.SetVelocity(Rand.Range(0f, 360f), Rand.Range(1f, 4f));

            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
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
                
            Thing myNewSpot = CreateMindFlaySpot(correctedCell, CasterPawn.Map);
            if(myNewSpot == null)
            {
                Tools.Warn("myNewSpot is null after CreateMindFlaySpot", myDebug);
                return;
            }

            Comp_LTF_MindFlaySpot MindFlaySpotComp = myNewSpot.TryGetComp<Comp_LTF_MindFlaySpot>();
            if(MindFlaySpotComp == null) { 
                Tools.Warn("MindFlaySpotComp is null after TryGetComp", myDebug);
                return;
            }

            MindFlaySpotComp.SetPawn(CasterPawn);

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