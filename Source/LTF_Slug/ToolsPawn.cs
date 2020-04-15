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
    public class ToolsPawn
    {
        public static BodyPartRecord GetBrain(Pawn pawn)
        {
            pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }

        public static BodyPartRecord GetStomach(Pawn pawn)
        {
            pawn.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Stomach).TryRandomElement(out BodyPartRecord bodyPart);
            return bodyPart;
        }

        public static void Warn(string warning, bool debug = false)
        {
            if(debug)
                Log.Warning(warning);
        }

    }
}
