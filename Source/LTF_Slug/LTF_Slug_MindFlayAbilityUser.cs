﻿using RimWorld;
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
    // reference: https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTUTORIAL%3A-JecsTools.CompAbilityUser

    public class CompMindFlayer : GenericCompAbilityUser 
    {
        public bool myDebug = true;

        public bool? MindFlayer;

        public bool EnableAbilities = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnableAbilities;

        // Provides ability without affecting save.
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
            if (MindFlayer == true)
                AddPawnAbility(MindFlayerDefOf.LTF_Slug_MindFlayer);
        }

        public bool IsMindFlayer
        {
            get
            {
                bool val = false;
                if (AbilityUser == null)
                    return false;

                // race
                val = AbilityUser.IsSlug();
                if (!val)
                {
                    Tools.Warn(AbilityUser.LabelShort + " is not Slug", myDebug);
                    return false;
                }

                // Natural Bodypart
                BodyPartRecord vestiShell = AbilityUser.GetVestiShell();
                if (vestiShell == null)
                {
                    Tools.Warn(AbilityUser.LabelShort + " has no vesti shell", myDebug);
                    return false;
                }

                return val;
            }
        }

        public override bool TryTransformPawn()
        {
            if (!EnableAbilities) return false;
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
                PawnAbility myAbility = AbilityData.AllPowers[i];
                yield return myAbility.GetGizmo();
                if (Prefs.DevMode)
                yield return new Command_Action
                {
                    defaultLabel = "reset " + myAbility.CooldownTicksLeft + " cooldown",
                    defaultDesc = "cooldown=" + myAbility.CooldownTicksLeft,
                    action = delegate
                    {
                        myAbility.CooldownTicksLeft = -1;
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