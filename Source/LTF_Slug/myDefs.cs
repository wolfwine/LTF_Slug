/*
 * Created by SharpDevelop.
 * User: Etienne
 * Date: 22/11/2017
 * Time: 16:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Verse;               // RimWorld universal objects are here (like 'Building')
using Verse.Sound;

using UnityEngine;

namespace LTF_Slug
{
    [StaticConstructorOnStartup]
    public static class MyDefs
    {
        public enum SpotKind
        {
            [Description("Flay spot")]
            flay = 0,
            [Description("Fondle spot")]
            fondle = 1,
            [Description("spot Error")]
            na = -99
        };

        public static HediffDef MindFlayHediff = DefDatabase<HediffDef>.AllDefs.Where((HediffDef b) => b.defName == "Hediff_LTF_MindFlay").RandomElement();
        public static HediffDef MindFondleHediff = DefDatabase<HediffDef>.AllDefs.Where((HediffDef b) => b.defName == "Hediff_LTF_MindFondle").RandomElement();

        public static ThoughtDef MindFlayThought = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef b) => b.defName == "LTF_MindFlayed_Thought").RandomElement();

        public static string MindFlaySpotName = "LTF_MindFlaySpot";
        public static string MindFondleSpotName = "LTF_MindFondleSpot";

        public static ThingDef MindFlaySpotThingDef = ThingDef.Named(MindFlaySpotName);
        public static ThingDef MindFondleSpotThingDef = ThingDef.Named(MindFondleSpotName);

        public static string FlayCompInspectStringExtra = "Flayage inflicted count: ";
        public static string FondleCompInspectStringExtra = "Cuddles count: ";

        public static readonly string vestigialShellName = "VestigialShellBP";
        public static readonly string fondlingVestigialShellName = "LTF_FondlingVestigialShell";

        public static readonly string slugDefName = "Alien_Slug";
    }
}
