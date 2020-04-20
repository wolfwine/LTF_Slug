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
    public static class MindSpotUtils
    {
        public static Thing CreateMindSpot(IntVec3 destinationCell, Map map, MyDefs.SpotKind spotKind)
        {
            ThingDef thingDef = null;

            if (spotKind == MyDefs.SpotKind.flay)
                thingDef = MyDefs.MindFlaySpotThingDef;
            else if (spotKind == MyDefs.SpotKind.fondle)
                thingDef = MyDefs.MindFondleSpotThingDef;
            else
            {
                Tools.Warn("CreateMindSpot - bad spotkind", true);
            }

            Building mindFlaySpot = (Building)ThingMaker.MakeThing(thingDef, null);

            //lockBlock.SetColor(lockDowner.DrawColor);
            GenSpawn.Spawn(mindFlaySpot, destinationCell, map, Rot4.North, WipeMode.Vanish);

            mindFlaySpot.SetFaction(Faction.OfPlayer);

            GfxEffects.ThrowCoupleMotes(destinationCell.ToVector3(), map, spotKind);

            foreach (IntVec3 puff in GenAdj.CellsAdjacent8Way(mindFlaySpot))
                if (puff.InBounds(map))
                    MoteMaker.ThrowAirPuffUp(puff.ToVector3(), map);

            return (mindFlaySpot);
        }

        public static Comp_LTF_MindSpot TryGetMindSpotComp(Thing thing) {
            Comp_LTF_MindSpot MindSpotComp = thing.TryGetComp<Comp_LTF_MindSpot>();
            if (MindSpotComp == null)
            {
                Tools.Warn("MindFlaySpotComp is null after TryGetComp", true);
                return null;
            }
            return MindSpotComp;
        }
    }
}
