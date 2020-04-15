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
    public class Tools
    {
        public static bool CheckPawn(Pawn pawn)
        {
            //return (pawn != null && pawn.Map != null && pawn.Position != null);
            return (pawn != null && pawn.Map != null);
        }
        public static void Warn(string warning, bool debug = false)
        {
            if(debug)
                Log.Warning(warning);
        }

    }
}
