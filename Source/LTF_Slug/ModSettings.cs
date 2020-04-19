﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;


namespace LTF_Slug
{
    public class LTF_SlugSettings : ModSettings
    {
        public bool EnableAbilities = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref EnableAbilities, "EnableAbilities");
        }

    }

    public class LTF_SlugMod : Mod
    {
        LTF_SlugSettings settings;

        public LTF_SlugMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<LTF_SlugSettings>();
        }

        public override string SettingsCategory()
        {
            return "LTF Slug";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            //listing.Label("Enable abilities: " + settings.EnableAbilities);
            listing.CheckboxLabeled("Enable abilities: ", ref settings.EnableAbilities);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}