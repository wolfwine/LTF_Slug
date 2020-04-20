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

        public enum ClosestColor
        {
            [Description("blue")]
            blue = 0,
            [Description("orange")]
            orange = 1,
            [Description("purple")]
            purple = 2
        }

        public static string[] moteFlayPool = {
            "Mote_MindFlay_Blue",
            "Mote_MindFlay_Orange",
            "Mote_MindFlay_Purple"
        };

        public static string[] moteFondlePool = {
            "Mote_MindFondle_Blue",
            "Mote_MindFondle_Orange",
            "Mote_MindFondle_Purple"
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

        // CutoutPlant MetaOverlay MoteGlow
        //Shader shtype = ShaderDatabase.CutoutSkin;
        // Flay underlay
        public static readonly Material FlayBlueUnderlay = MaterialPool.MatFrom(overlayPath + "FlayBlueUnderlay", ShaderDatabase.CutoutSkin);
        public static readonly Material FlayOrangeUnderlay = MaterialPool.MatFrom(overlayPath + "FlayOrangeUnderlay", ShaderDatabase.CutoutSkin);
        public static readonly Material FlayPurpleUnderlay = MaterialPool.MatFrom(overlayPath + "FlayPurpleUnderlay", ShaderDatabase.CutoutSkin);
        // Flay overlay
        public static readonly Material FlayOverlay = MaterialPool.MatFrom(overlayPath + "FlayOverlay", ShaderDatabase.MetaOverlay);

        // Fondle underlay
        public static readonly Material FondleBlueUnderlay = MaterialPool.MatFrom(overlayPath + "FondleBlueUnderlay", ShaderDatabase.CutoutSkin);
        public static readonly Material FondleOrangeUnderlay = MaterialPool.MatFrom(overlayPath + "FondleOrangeUnderlay", ShaderDatabase.CutoutSkin);
        public static readonly Material FondlePurpleUnderlay = MaterialPool.MatFrom(overlayPath + "FondlePurpleUnderlay", ShaderDatabase.CutoutSkin);
        // Fondle overlay
        public static readonly Material FondleBlueOverlay = MaterialPool.MatFrom(overlayPath + "FondleBlueOverlay", ShaderDatabase.MoteGlow);
        public static readonly Material FondleOrangeOverlay = MaterialPool.MatFrom(overlayPath + "FondleOrangeOverlay", ShaderDatabase.MoteGlow);
        public static readonly Material FondlePurpleOverlay = MaterialPool.MatFrom(overlayPath + "FondlePurpleOverlay", ShaderDatabase.MoteGlow);
    }
}
