using UnityEngine;
using Verse;

namespace GNATFramework
{
    public class GNATSettings : ModSettings
    {
        public static bool reuseNeoAmmo = true;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref reuseNeoAmmo, "reuseNeoAmmo");
            base.ExposeData();
        }
    }
    public class GNATMod : Mod
    {
        public GNATMod(ModContentPack content) : base(content)
        {
            GetSettings<GNATSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("GNAT_NeoAmmo_Label".Translate(), ref GNATSettings.reuseNeoAmmo, "GNAT_NeoAmmo_ToolTip".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
        public override string SettingsCategory() => "GNAT_Settings".Translate();
    }
}
