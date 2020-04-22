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
    // reference: https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTUTORIAL%3A-JecsTools.CompAbilityUser

    public class CompMindFlayer : GenericCompAbilityUser 
    {
        public bool myDebug = false;

        public bool? MindFlayer;

        public bool EnableAbilities = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnableAbilities;
        public int period = 600;

        // Provides ability without affecting save.
        public override void CompTick()
        {
            //Tools.Warn(AbilityUser.Label + " CompMindFlayer.CompTick", myDebug);
            
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
                    Tools.Warn(AbilityUser.LabelShort + " Trying to transform into MindFlayer", myDebug);
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
                if (AbilityUser == null)
                    return false;

                string userLabel = (myDebug)?AbilityUser.LabelShort : "";

                // race
                if (!AbilityUser.IsSlug())
                {
                    Tools.Warn(userLabel + " is not Slug, giving up IsMindFlayer" + "\n-----", myDebug);
                    return false;
                }
                else
                {
                    Tools.Warn(userLabel + " is indeed Slug", myDebug);
                }

                // Natural Bodypart
                if (!AbilityUser.HasNaturalVestigialShell(myDebug))
                {
                    Tools.Warn(userLabel + " has no natural vestigial shell, giving up IsMindFlayer"+"\n-----", myDebug);
                    return false;
                }
                else
                {
                    Tools.Warn(userLabel + " has indeed a natural vestigial shell", myDebug);
                }


                Tools.Warn(userLabel + " IsMindFlayer"+"\n-----", myDebug);
                return true;
            }
        }

        public override bool TryTransformPawn()
        {
            if (!EnableAbilities) return false;
            return IsMindFlayer;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (!AbilityUser.IsSleepingOrOnFire())
            {
                IEnumerator<Gizmo> gizmoEnum = base.CompGetGizmosExtra().GetEnumerator();
                while (gizmoEnum.MoveNext())
                {
                    Gizmo current = gizmoEnum.Current;
                    yield return current;
                }

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