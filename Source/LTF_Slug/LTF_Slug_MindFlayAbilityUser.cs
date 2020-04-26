using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using AbilityUser;

namespace LTF_Slug
{
    [StaticConstructorOnStartup]
    // reference: https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTUTORIAL%3A-JecsTools.CompAbilityUser

    public class CompMindFlayer : GenericCompAbilityUser 
    {
        public bool myDebug = false;

        public bool? MindFlayer;

        public bool EnableAbilities = LoadedModManager.GetMod<LTF_SlugMod>().GetSettings<LTF_SlugSettings>().EnableAbilities;

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
            {
                Tools.Warn(AbilityUser.LabelShort + " adding MindFlayer ability", myDebug);
                AddPawnAbility(MindFlayerDefOf.LTF_Slug_MindFlayer);
            }
        }

        public void TryRemoveMindFlayer()
        {
            CompMindFlayer checkIfMindFlayer = AbilityUser.TryGetComp<CompMindFlayer>();
            if (checkIfMindFlayer != null)
            {
                Tools.Warn(AbilityUser.LabelShort + " removing MindFlayer ability", myDebug);
                RemovePawnAbility(MindFlayerDefOf.LTF_Slug_MindFlayer);
            }
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
                    Tools.Warn(userLabel + " has no natural vestigial shell, trying to remove Flayer ability"+"\n-----", myDebug);

                    TryRemoveMindFlayer();

                    return false;
                }
                else
                {
                    Tools.Warn(userLabel + " has indeed a natural vestigial shell", myDebug);
                }


                Tools.Warn(userLabel + " should become MindFlayer", myDebug);
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