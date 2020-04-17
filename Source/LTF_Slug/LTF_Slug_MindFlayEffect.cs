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
            Tools.Warn("CanHitTargetFrom : Blink in TryCastShot with root" + root + " and target" + targ, myDebug);

            if (UseAbilityProps.AbilityTargetCategory == AbilityTargetCategory.TargetLocation)
            {
                Tools.Warn("AbilityTargetCategory == TargetLocation; base.CanHitTargetFrom", myDebug);
                return base.CanHitTargetFrom(root, targ);
            }
            Tools.Warn("??? AbilityTargetCategory != TargetLocation; TryFindShootLineFromTo", myDebug);

            return TryFindShootLineFromTo(root, targ, out ShootLine shootLine);
        }

        protected override bool TryCastShot()
        {
            Tools.Warn("Entering TryCastShot", myDebug);

            if (currentTarget != null && CasterPawn != null && currentTarget.Cell != null && currentTarget.Cell.IsValid)
            {
                Tools.Warn("Valid target in TryCastShot", myDebug);
                CreateMindFlaySpot(currentTarget.Cell, CasterPawn.Map);
                Tools.Warn("created MindFlaySpot in TryCastShot", myDebug);
                return true;
                //this.CasterPawn.SetPositionDirect(this.currentTarget.Cell);
            }
            else
            {
                Tools.Warn("failed to TryCastShot", myDebug);
            }

            // Setting cooldown
            //ability.TicksUntilCasting = (int)this.UseAbilityProps.SecondsToRecharge * GenTicks.TicksPerRealSecond;

            return false;
        }
        /*
        public virtual void Effect()
        {
            string message = string.Empty;

            CreateLockBlocks(CasterPawn);
            MoteMaker.ThrowText(CasterPawn.DrawPos, CasterPawn.Map, message);
        }
        */

        //IntVec3
        public static IntVec3 InvertIntVec3(IntVec3 intVec3)
        {
            IntVec3 temp;
            int x, y, z;
            x = intVec3.x; y = intVec3.y; z = intVec3.z;

            x *= -1;
            z *= -1;

            temp = new IntVec3(x, y, z);
            return (temp);
        }
        
        public static void CreateMindFlaySpot(IntVec3 destinationCell, Map map)
        {
            Building mindFlaySpot = (Building)ThingMaker.MakeThing(ThingDef.Named("LTF_MindFlaySpot"), null);

            //lockBlock.SetColor(lockDowner.DrawColor);
            GenSpawn.Spawn(mindFlaySpot, destinationCell, map, Rot4.North, WipeMode.Vanish);

            ThrowMicroFlakes(destinationCell.ToVector3(), map);

            foreach (IntVec3 puff in GenAdj.CellsAdjacent8Way(destinationCell, Rot4.North, IntVec2.North))
                if(puff.InBounds(map))
                    MoteMaker.ThrowAirPuffUp(puff.ToVector3(), map);
        }

        public static void ThrowMicroFlakes(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("Mote_MindFlay"), null);
            moteThrown.Scale = Rand.Range(0.5f, 1.2f);
            moteThrown.rotationRate = Rand.Range(-12f, 36f);

            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocity((float)Rand.Range(35, 45), 1.2f);

            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Necessary for autocomplete ability
        public override void PostCastShot(bool inResult, out bool outResult)
        {
            if (inResult)
            {
                //Effect();
                outResult = true;
            }
            outResult = inResult;
        }
    }
}