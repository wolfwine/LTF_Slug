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
    public static class MyGfx
    {

        public static string basePath = "Things/AbilityBuilding/";
        public static string overlayPath = basePath + "GfxEffects/";
        public static string underlayPath = basePath + "GfxEffects/";

        public enum ClosestColor
        {
            blue = 0,
            orange = 1,
            purple = 2
        }

        public static string[] motePool = {
            "Mote_MindFlay_Blue",
            "Mote_MindFlay_Orange",
            "Mote_MindFlay_Purple"
        };

        public static Color[] ChosenColors =
        {
            // blue
            new Color(31,117, 254),
            //orange
            new Color(225, 115, 39),
            // redish Indigo
            new Color(201, 27, 38)
        };

        public static readonly Material BlueUnderlayM = MaterialPool.MatFrom(underlayPath + "BlueSpiral", ShaderDatabase.MoteGlow);
        public static readonly Material OrangeUnderlayM = MaterialPool.MatFrom(underlayPath + "OrangeSpiral", ShaderDatabase.MoteGlow);
        public static readonly Material PurpleUnderlayM = MaterialPool.MatFrom(underlayPath + "PurpleSpiral", ShaderDatabase.MoteGlow);

        public static readonly Material BlueOverlayM = MaterialPool.MatFrom(overlayPath + "BlueSpiralPlus", ShaderDatabase.MetaOverlay);
        public static readonly Material OrangeOverlayM = MaterialPool.MatFrom(overlayPath + "OrangeSpiralPlus", ShaderDatabase.MetaOverlay);
        public static readonly Material PurpleOverlayM = MaterialPool.MatFrom(overlayPath + "PurpleSpiralPlus", ShaderDatabase.MetaOverlay);
    }
}
