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

        public bool IsMindFlayer
        {
            get
            {
                bool val = false;
                if (this.AbilityUser == null)
                    return false;

                // race
                //val = ToolsPawn.IsSlug(AbilityUser);
                val = AbilityUser.IsSlug();
                if (!val)
                {
                    Tools.Warn(AbilityUser.LabelShort + " is not Slug", myDebug);
                    return false;
                }

                // heart
                BodyPartRecord vestiShell = AbilityUser.GetVestiShell();
                if (vestiShell == null)
                {
                    Tools.Warn(AbilityUser.LabelShort + " has no vesti shell", myDebug);
                    return false;
                }

                // crystal clear health
                /*
                List<Hediff> myHediffs = AbilityUser.health.hediffSet.hediffs;
                for (int i = 0; i < myHediffs.Count; i++)
                {
                    BodyPartRecord myBP = myHediffs[i].Part;
                    */
                return val;
            }
        }

        public override bool TryTransformPawn()
        {
            //Log.Warning("TryTransformPawn");
            //if (!LockdownerMod.settings.enableLockdownerAbility) return false;
            return IsMindFlayer;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            IEnumerator<Gizmo> gizmoEnum = base.CompGetGizmosExtra().GetEnumerator();
            while (gizmoEnum.MoveNext())
            {
                Gizmo current = gizmoEnum.Current;
                yield return current;
            }
            //if (AbilityUser.Drafted)
            if (!AbilityUser.IsSleepingOrOnFire())
            for (int i = 0; i < AbilityData.AllPowers.Count; i++)
            {
                yield return AbilityData.AllPowers[i].GetGizmo();
                if(Prefs.DevMode)
                yield return new Command_Action
                {
                    defaultLabel = "reset " + AbilityData.AllPowers[i].CooldownTicksLeft + " cooldown",
                    defaultDesc = "cooldown=" + AbilityData.AllPowers[i].CooldownTicksLeft,
                    action = delegate
                    {
                        AbilityData.AllPowers[i].CooldownTicksLeft = -1;
                    }
                };
            }
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