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
        public bool myDebug = true;

        public bool? MindFondler;

        public bool EnableAbilities = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnableAbilities;

        // Provides ability without affecting save.
        public override void CompTick()
        {
            //Log.Warning("CompTick");
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