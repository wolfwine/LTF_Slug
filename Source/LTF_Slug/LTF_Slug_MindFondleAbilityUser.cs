using RimWorld;
using System.Collections.Generic;
using Verse;
using AbilityUser;


namespace LTF_Slug
{
    [StaticConstructorOnStartup]
    // reference: https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTUTORIAL%3A-JecsTools.CompAbilityUser

    public class CompMindFondler : GenericCompAbilityUser 
    {
        public bool myDebug = false;

        public bool? MindFondler;

        public bool EnableAbilities = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnableAbilities;

        // Provides ability without affecting save.
        public override void CompTick()
        {
            //Tools.Warn(AbilityUser.Label + " CompMindFondler.CompTick", myDebug);
            if (AbilityUser?.Spawned == true)
            {
                if (MindFondler != null)
                {
                    if (MindFondler == true)
                    {
                        base.CompTick();
                    }
                }
                else
                {
                    Tools.Warn(AbilityUser.LabelShort + " Trying to transform into MindFondler", myDebug);
                    MindFondler = TryTransformPawn();
                    Initialize();
                }
            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            if (MindFondler == true)
                AddPawnAbility(MindFondlerDefOf.LTF_Slug_MindFondler);
        }

        public bool IsMindFondler
        {
            get
            {
                if (AbilityUser == null)
                    return false;

                string userLabel = (myDebug) ? AbilityUser.LabelShort : "";

                // race
                if (!AbilityUser.IsSlug())
                {
                    Tools.Warn(userLabel + " is not Slug, giving up IsMindFondler" + "\n-----", myDebug);
                    return false;
                }
                else
                {
                    Tools.Warn(userLabel + " is indeed Slug", myDebug);
                }

                // Natural Bodypart
                if (!AbilityUser.HasFondlingVestigialShell(myDebug))
                {
                    Tools.Warn(userLabel + " has no fondling vestigial shell, giving up IsMindFondler" + "\n-----", myDebug);
                    return false;
                }
                else
                {
                    Tools.Warn(userLabel + " has indeed a fondling vestigial shell", myDebug);
                }

                Tools.Warn(userLabel + " IsMindFondler" + "\n-----", myDebug);
                return true;
            }
        }

        public override bool TryTransformPawn()
        {
            if (!EnableAbilities) return false;
            return IsMindFondler;
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
    public static class MindFondlerDefOf
    {
        public static AbilityUser.AbilityDef LTF_Slug_MindFondler;
    }
    
    public class MindFondler_Projectile : AbilityUser.Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
        }
    }
    
}