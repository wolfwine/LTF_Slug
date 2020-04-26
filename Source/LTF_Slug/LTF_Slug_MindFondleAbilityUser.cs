using RimWorld;
using System.Collections.Generic;
using System.Linq;
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
            {
                Tools.Warn(AbilityUser.LabelShort + " adding MindFondler ability", myDebug);
                AddPawnAbility(MindFondlerDefOf.LTF_Slug_MindFondler);
            }
            else
            {
                CompMindFondler checkIfMindFondler = AbilityUser.TryGetComp<CompMindFondler>();
                if (checkIfMindFondler != null)
                {
                    Tools.Warn(AbilityUser.LabelShort + " removing MindFondler ability", myDebug);
                    RemovePawnAbility(MindFondlerDefOf.LTF_Slug_MindFondler);
                }
            }
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

                Tools.Warn(userLabel + " should become MindFondler", myDebug);
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
            if (ToolsPawn.IsSlug(AbilityUser))
            {

                if (!AbilityUser.IsSleepingOrOnFire())
                {
                    IEnumerator<Gizmo> gizmoEnum = base.CompGetGizmosExtra().GetEnumerator();
                    while (gizmoEnum.MoveNext())
                    {
                        Gizmo current = gizmoEnum.Current;
                        yield return current;
                    }

                    IEnumerator<Gizmo> gizmoAbilities = ToolsAbilities.GetAbilityGizmos(AbilityData);
                    while (gizmoAbilities.MoveNext())
                    {
                        Gizmo current = gizmoAbilities.Current;
                        yield return current;
                    }
                }
                IEnumerable<Gizmo> reportGizmo = ToolsAbilities.GetAbilityReportGizmo(AbilityData);
                if (!reportGizmo.EnumerableNullOrEmpty())
                    yield return ToolsAbilities.GetAbilityReportGizmo(AbilityData).First();
            }
        }
    }

    [DefOf]
    public static class MindFondlerDefOf
    {
        public static AbilityUser.AbilityDef LTF_Slug_MindFondler;
    }
    
    public class MindFondler_Projectile : Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
        }
    }
    
}