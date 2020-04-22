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
    public static class ToolsPawn
    {
        public static bool CheckPawn(Pawn pawn)
        {
            return (pawn != null && pawn.Map != null);
        }

        public static bool IsSlug(this Pawn pawn)
        {
            return (pawn?.def.defName == MyDefs.slugDefName);
        }

        public static bool IsSleepingOrOnFire(this Pawn pawn)
        {
            if (pawn.CurJobDef == JobDefOf.LayDown || pawn.CurJobDef == JobDefOf.Wait_Downed)
                return true;

            if (pawn.HasAttachment(ThingDefOf.Fire) || pawn.CurJobDef == JobDefOf.ExtinguishSelf)
                return true;

            return false;
        }

        // -10% rest
        public static void ApplyTiredness(this Pawn pawn)
        {
            pawn.needs.rest.CurLevel = pawn.needs.rest.CurLevel * .9f;
        }
    }
}
