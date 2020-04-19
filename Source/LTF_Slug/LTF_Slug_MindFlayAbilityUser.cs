using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using AbilityUser;
using AlienRace;
using UnityEngine;

using System;
using System.Diagnostics;
using System.Text;

using Verse.Sound;

namespace LTF_Slug
{
    [StaticConstructorOnStartup]
    public static class TexButton
    {
        public static readonly Texture2D MindFlayerAbility = ContentFinder<Texture2D>.Get("UI/MindFlay", true);
    }

    // reference: https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTUTORIAL%3A-JecsTools.CompAbilityUser

    public class CompMindFlayer : GenericCompAbilityUser 
    {
        public bool myDebug = true;

        public bool? MindFlayer;
        
        // Provides ability without affecting save.a
        public override void CompTick()
        {
            //Log.Warning("CompTick");
            if (AbilityUser?.Spawned == true)
            {
                if (MindFlayer != null)
                {
                    if (MindFlayer == true)
                    {
                        base.CompTick();
                    }
                }
                else
                {
                    MindFlayer = TryTransformPawn();
                    Initialize();
                }
            }

        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            if (MindFlayer == true) this.AddPawnAbility(MindFlayerDefOf.LTF_Slug_MindFlayer);
        }

        private BodyPartRecord GetHeart(Pawn pawn)
        {
            BodyPartRecord bodyPart = null;
            //pawn.RaceProps.body.GetPartsWithTag("BloodPumpingSource").TryRandomElement(out bodyPart);
            pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BloodPumpingSource).TryRandomElement(out bodyPart);
            if (bodyPart == null)
            {
                Tools.Warn("null heart ?", myDebug);
            }

            return bodyPart;
        }


        public bool IsMindFlayer
        {
            get
            {
                bool val = false;
                if (this.AbilityUser == null)
                    return false;

                // race
                val = IsSlug(AbilityUser);
                if (!val)
                {
                    Tools.Warn(AbilityUser.LabelShort + " is not Slug", myDebug);
                    return false;
                }

                // heart
                BodyPartRecord heart = GetHeart(AbilityUser);
                if (heart == null)
                {
                    Tools.Warn(AbilityUser.LabelShort + " has no heart", myDebug);
                    return false;
                }

                // crysta heart
                if (heart.def.defName != "Heart")
                {
                    Tools.Warn(AbilityUser.LabelShort + " has a non legit " + heart.def.defName, myDebug);
                    return false;
                }


                // crystal clear health
                List<Hediff> myHediffs = AbilityUser.health.hediffSet.hediffs;
                for (int i = 0; i < myHediffs.Count; i++)
                {
                    BodyPartRecord myBP = myHediffs[i].Part;

                    if (myBP != heart) {
                        continue;
                    }
                    else
                    {
                        Tools.Warn(AbilityUser.LabelShort + " is special", myDebug);
                        val = false;
                        break;
                    }
                }

                return val;
            }
        }


        private bool IsSlug(Pawn pawn)
        {
            return (pawn?.def.defName == "Alien_Slug");
        }

        public override bool TryTransformPawn()
        {
            //Log.Warning("TryTransformPawn");
            //if (!LockdownerMod.settings.enableLockdownerAbility) return false;
            return IsMindFlayer;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            //if (AbilityUser.Drafted)
            
            IEnumerator<Gizmo> enumerator = base.CompGetGizmosExtra().GetEnumerator();
            while (enumerator.MoveNext())
            {
                Gizmo current = enumerator.Current;
                yield return current;
            }
            for (int i = 0; i < this.AbilityData.AllPowers.Count; i++)
                yield return this.AbilityData.AllPowers[i].GetGizmo();
            
        }
    }

    [DefOf]
    public static class MindFlayerDefOf
    {
        public static AbilityUser.AbilityDef LTF_Slug_MindFlayer;
    }

    public class MindFlayer_Projectile : AbilityUser.Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
        }
    }
}