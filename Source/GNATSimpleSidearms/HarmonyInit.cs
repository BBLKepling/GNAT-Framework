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
            Log.Message("[GNAT]Simple sidearms detected, Harmony Patching GNATFramework.Verb_LaunchProjectileOneUse");
        }
    }
}
