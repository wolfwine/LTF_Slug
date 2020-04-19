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
    public static class GfxEffects
    {
        public enum Layer
        {
            over = 4,
            under = -1,
        };

        public static ThingDef RandomMote
        {
            get
            {
                return (ThingDef.Named(MyGfx.motePool[(int)Rand.Range(0, MyGfx.motePool.Length)]));
            }
        }

        public static void ThrowMindFondleMote(Vector3 loc, Map map)
        {
            ThrowMindFlayMote(loc, map);
        }

        public static void ThrowMindFlayMote(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            //MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("Mote_MindFlay"), null);
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(RandomMote);
            moteThrown.Scale = Rand.Range(0.5f, 1.2f);

            moteThrown.rotationRate = Rand.Range(0, 180);

            moteThrown.exactPosition = loc;
            /*
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            */

            moteThrown.SetVelocity(Rand.Range(0f, 360f), Rand.Range(1f, 4f));

            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        public static float VanillaPulse(Thing thing)
        {
            float num = (Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f;
            float num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
            num2 = 0.3f + num2 * 0.7f;
            return num2;
        }

        public static float LoopAroundOneNormal(Thing thing, bool myDebug = false)
        {
            return LoopFactorOne(thing, 1, .5f, myDebug);
        }

        public static float LoopAroundOneSlow(Thing thing, bool myDebug = false)
        {
            return LoopFactorOne(thing, 1, .2f, myDebug);
        }

        public static float LoopAroundOneSuperSlow(Thing thing, bool myDebug = false)
        {
            return LoopFactorOne(thing, 1, .05f, myDebug);
        }

        public static float LoopFactorOne(Thing thing, float mask = 1, float speedUp=1f, bool myDebug = false)
        {
            float timePhaseShiftValue = 397f;
            float thingPhaseShiftValue = 571f;

            float num = (Time.realtimeSinceStartup + timePhaseShiftValue * (float)(thing.thingIDNumber % thingPhaseShiftValue)) * speedUp;
            //float num2 = (float)Math.Sin((double)num);
            float num2 = num % 1;
            Tools.Warn("loop factor one" + num2 + "; mask: " + mask + "; masked: " + num % mask, myDebug);
            num2 = num2 % mask;
            return num2;
        }

        public static float PulseFactorOne(Thing thing, float mask = 1, bool debug = false)
        {
            float timePhaseShiftValue = 397f;
            float thingPhaseShiftValue = 571f;
            float speedUp = 2f;

            float num = (Time.realtimeSinceStartup + timePhaseShiftValue * (float)(thing.thingIDNumber % thingPhaseShiftValue)) * speedUp;
            float num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
            Tools.Warn("pulse factor one: " + num2 + "; mask: " + mask + "; masked: " + num % mask, debug);
            num2 = num2 % mask;

            return num2;
        }

        public static void DrawTickRotating(Thing thing, Material mat, float x, float z, float size = 1f, float angle = 0f, float opacity = 1, Layer myLayer = Layer.over, bool myDebug = false)
        {
            Vector3 dotS = new Vector3(size, 1f, size);

            Matrix4x4 matrix = default(Matrix4x4);
            Vector3 dotPos = thing.TrueCenter();

            dotPos.x += x;
            dotPos.z += z;
            dotPos.y += (float)myLayer;

            Material fadedMat = mat;

            if (opacity != 1)
                fadedMat = FadedMaterialPool.FadedVersionOf(mat, opacity);

            Tools.Warn("Drawing - ang: " + angle + "; opa:" + opacity, myDebug);
            matrix.SetTRS(dotPos, Quaternion.AngleAxis(angle, Vector3.up), dotS);
            Graphics.DrawMesh(MeshPool.plane10, matrix, fadedMat, 0);
        }

        // closed match in RGB space
        public static MyGfx.ClosestColor ClosestColor(Building building, bool myDebug = false)
        {
            Color bColor = building.DrawColor;

            MyGfx.ClosestColor answer = MyGfx.ClosestColor.blue;

            int blueDiff, orangeDiff, purpleDiff;
            int minVal = 1000;

            int bColorRed = (int)(bColor.r * 256);
            int bColorGreen = (int)(bColor.g * 256);
            int bColorBlue = (int)(bColor.b * 256);

            blueDiff = (
                Math.Abs(bColorRed - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.blue].r) / 3 +
                Math.Abs(bColorGreen - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.blue].g) / 2 +
                Math.Abs(bColorBlue - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.blue].b)
            );

            orangeDiff = (
                Math.Abs(bColorRed - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.orange].r) +
                Math.Abs(bColorGreen - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.orange].g) / 2 +
                Math.Abs(bColorBlue - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.orange].b) / 3
            );

            purpleDiff = (
                Math.Abs(bColorRed - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.purple].r) +
                Math.Abs(bColorGreen - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.purple].g) / 3 +
                Math.Abs(bColorBlue - (int)MyGfx.ChosenColors[(int)MyGfx.ClosestColor.purple].b) / 2
            );

            Tools.Warn(
                "bColor: " + bColor +
                "; blue: " + MyGfx.ChosenColors[(int)MyGfx.ClosestColor.blue] +
                "; orange: " + MyGfx.ChosenColors[(int)MyGfx.ClosestColor.orange] +
                "; purple: " + MyGfx.ChosenColors[(int)MyGfx.ClosestColor.purple]
                , myDebug
            );
            Tools.Warn(
                "blueDiff: " + blueDiff +
                "; orangeDiff: " + orangeDiff +
                "; purpleDiff: " + purpleDiff
                , myDebug
            );

            if (blueDiff < minVal)
            {
                minVal = blueDiff;
                answer = MyGfx.ClosestColor.blue;
            }

            if (orangeDiff < minVal)
            {
                minVal = orangeDiff;
                answer = MyGfx.ClosestColor.orange;
            }

            if (purpleDiff < minVal)
            {
                minVal = purpleDiff;
                answer = MyGfx.ClosestColor.purple;
            }

            return answer;
        }
    }
}
