using GNATFramework;
using HarmonyLib;
using Verse;

namespace GNATSimpleSidearms
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmonyInstance = new Harmony("BBLKepling.GNAT.SimpleSidearms");
            harmonyInstance.PatchAll();
            if (GNATSettings.logSpam) Log.Message("GNAT_LaunchProjectileOneUse".Translate());
        }
    }
}
