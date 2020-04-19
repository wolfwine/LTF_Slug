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
    public class MyGizmo
    {
        //static string basePath = "Things/Building/TpSpot/";
        static string GizmoPath = "UI/Commands/";
        static string DebugPath = GizmoPath + "Debug/";

        //Common
        public static Texture2D DebugOnGz = ContentFinder<Texture2D>.Get(DebugPath + "DebugOn", true);
        public static Texture2D DebugOffGz = ContentFinder<Texture2D>.Get(DebugPath + "DebugOff", true);
        public static Texture2D DebugLogGz = ContentFinder<Texture2D>.Get(DebugPath + "DebugLog", true);

    }
}
