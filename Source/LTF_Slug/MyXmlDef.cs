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
    public class MyXmlDef
    {
        public static HediffDef MindFlayedHediff = DefDatabase<HediffDef>.AllDefs.Where((HediffDef b) => b.defName == "Hediff_LTF_MindFlayed").RandomElement();
        public static ThoughtDef MindFlayedThought = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef b) => b.defName == "LTF_MindFlayed_Thought").RandomElement();
    }
}
