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
    public class ToolsCell
    {
        public static bool CellHasBuildingOrMaturePlant(IntVec3 myCell, Map myMap, bool myDebug = false)
        {
            foreach (Thing myThing in myCell.GetThingList(myMap))
            {
                Tools.Warn(" CellHasBuilding checkin "+ myCell + ": "+ myThing.Label +"("+ myThing.def.defName + ")" , myDebug);
                if (myThing is Building)
                {
                    Tools.Warn(myThing.Label + " is a building", myDebug);
                    return true;
                }

                if (myThing is Plant)
                {
                    if( myThing.def.defName.Contains("Plant_Tree"))
                    {
                        Plant myPlant = (Plant)myThing;
                        if (myPlant.Growth > .5f)
                        {
                            Tools.Warn(myThing.Label + " is a mature enough tree", myDebug);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static List<IntVec3> GenCellsBetween(IntVec3 source, IntVec3 destination, bool myDebug = false)
        {
            List<IntVec3> cellList = new List<IntVec3> { };

            IntVec3 diffVector = source - destination;

            Tools.Warn("GenCellsBetween: " + source + "->" + destination + "; diffV:" + diffVector, myDebug);

            // points are close to each other; no need to gen anything
            if ((Math.Abs(diffVector.x) <= 1) && (Math.Abs(diffVector.z) <= 1))
            {
                cellList.Add(source);
                return cellList;
            }

            // the line start from destination and goes to source
            // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
            int dx = Math.Abs(source.x - destination.x);
            int sx = (destination.x < source.x) ? 1 : -1;
            int dy = -Math.Abs(source.z - destination.z);
            int sy = (destination.z < source.z) ? 1 : -1;
            int err = dx + dy;

            int newX = destination.x;
            int newY = destination.z;

            int loopBreaker = 20;
            while (true) {
                Tools.Warn((20-loopBreaker)+" -> (" + newX + ";" + newY + ")", myDebug);
                cellList.Add(new IntVec3(newX, 0, newY));

                if (newX == source.x && newY == source.z) break;
                int e2 = 2 * err;
                if (e2 >= dy) {
                    err += dy;
                    newX += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    newY += sy;
                }
                if (loopBreaker-- <= 0)
                    break;
            }
            Tools.Warn("generated " + cellList.Count + " cells", myDebug);
             
            return cellList;
        }
        public static IntVec3 GetCloserCell(IntVec3 source, IntVec3 destination, Map myMap, bool myDebug = false)
        {
            IntVec3 answer = IntVec3.Zero;

            //IntVec3[] myCellArray = new IntVec3[30];

            List <IntVec3> cellList = GenCellsBetween(source, destination, myDebug);

            //if (myCellArray.NullOrEmpty())
            if (cellList.NullOrEmpty())
            {
                Tools.Warn("cellList.NullOrEmpty in GetCloserCell", myDebug);
                return answer;
            }

            Tools.Warn(cellList.Count + " cells generated", myDebug);
            //foreach (IntVec3 myCell in myCellArray)
            foreach (IntVec3 myCell in cellList)
            {
                if (!CellHasBuildingOrMaturePlant(myCell, myMap, myDebug))
                    return myCell;
            }

            return answer;
        }

        public static List<Pawn> GetPawnsInRadius(
            IntVec3 center, float radius, Map myMap,
            bool affectsAnimals = false, bool affectsHumanlike = true, bool affectsMechanoids = false,
            bool affectsColonists = false, bool affectsNeutralOrFriends = false, bool affectsEnemies = true,
            bool myDebug = false)
        {
            List<Pawn> pawnList = new List<Pawn> { };

            IEnumerable<IntVec3> inRangeCells = GenRadial.RadialCellsAround(center, radius, true);

            foreach (IntVec3 myCell in inRangeCells)
            {
                if (!myCell.InBounds(myMap))
                    continue;

                if (myCell.Impassable(myMap))
                    continue;

                foreach (Thing thing in myCell.GetThingList(myMap))
                {
                    if (thing is Pawn curPawn)
                    {
                        string DebugStr = "GetPawnsInRadius - " + curPawn.Label;

                        // animals
                        if (affectsAnimals && !curPawn.RaceProps.Animal) 
                        {
                            Tools.Warn(DebugStr + " is not animal", myDebug);
                            continue;
                        }
                        else if (!affectsAnimals && curPawn.RaceProps.Animal)
                        {
                            Tools.Warn(DebugStr + " is animal", myDebug);
                            continue;
                        }

                        // humanlike
                        if (affectsHumanlike && !curPawn.RaceProps.Humanlike)
                        {
                            Tools.Warn(DebugStr + " is non humanlike", myDebug);
                            continue;
                        }
                        else if (!affectsHumanlike && curPawn.RaceProps.Humanlike)
                        {
                            Tools.Warn(DebugStr + " is humanlike", myDebug);
                            continue;
                        }

                        // mechanoids
                        if (affectsMechanoids && !curPawn.RaceProps.IsMechanoid) {
                            Tools.Warn(DebugStr + " is non mechanoid", myDebug);
                            continue;
                        }
                        else if (!affectsMechanoids && curPawn.RaceProps.IsMechanoid)
                        {
                            Tools.Warn(DebugStr + " is mechanoid", myDebug);
                            continue;
                        }
                        

                        if (curPawn.Faction != null)
                        {
                            // colonists or colony animal
                            if (affectsColonists && !curPawn.Faction.IsPlayer) {
                                Tools.Warn(DebugStr + " is not colonist nor colony animal", myDebug);
                                continue;
                            }
                            else if (!affectsColonists && curPawn.Faction.IsPlayer)
                            {
                                Tools.Warn(DebugStr + " is colonist or colony animal", myDebug);
                                continue;
                            }

                            // ally or neutral
                            if (!curPawn.Faction.IsPlayer)
                            {
                                if (affectsNeutralOrFriends && !curPawn.Faction.AllyOrNeutralTo(Faction.OfPlayer)){
                                    Tools.Warn(DebugStr + " is not ally or neutral", myDebug);
                                    continue;
                                }
                                else if (!affectsNeutralOrFriends && curPawn.Faction.AllyOrNeutralTo(Faction.OfPlayer))
                                {
                                    Tools.Warn(DebugStr + " is ally or neutral", myDebug);
                                    continue;
                                }
                            }

                            // enemy
                            if (affectsEnemies && curPawn.Faction.AllyOrNeutralTo(Faction.OfPlayer)){
                                Tools.Warn(DebugStr + " is not enemy", myDebug);
                                continue;
                            } else if (!affectsEnemies && !curPawn.Faction.AllyOrNeutralTo(Faction.OfPlayer)) {
                                Tools.Warn(DebugStr + " is enemy", myDebug);
                                continue;
                            }
                        }

                        Tools.Warn(DebugStr + " is OK", myDebug);
                        pawnList.Add(curPawn);
                    }
                }
            }

            return pawnList;
        }
    }
}
