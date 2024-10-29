using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace GNATFramework
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        public static bool ssInstalled;
        static HarmonyInit()
        {
            ssInstalled = ModLister.HasActiveModWithName("Simple sidearms");
            List<string> features = new List<string>();
            foreach (ModFeatureDef def in DefDatabase<ModFeatureDef>.AllDefs)
            {
                foreach (string feature in def.features)
                {
                    features.Add(feature.ToLower());
                }
            }
            if (features.NullOrEmpty()) return;
            if (GNATSettings.logSpam) Log.Message("GNAT_Start_Up".Translate());
            Harmony harmonyInstance = new Harmony("BBLKepling.GNAT");
            if (features.Contains("generatewithequip"))
            {
                MethodInfo original = AccessTools.Method(typeof(PawnWeaponGenerator), nameof(PawnWeaponGenerator.TryGenerateWeaponFor));
                MethodInfo postfix = typeof(Harmony_PawnWeaponGenerator_TryGenerateWeaponFor_Postfix).GetMethod("TryGenerateWeaponFor");
                if (GNATSettings.logSpam) Log.Message("GNAT_GenerateWithEquip".Translate());
                harmonyInstance.Patch(original: original, postfix: new HarmonyMethod(postfix));
            }
            if (features.Contains("shootoneuseinvcheck"))
                if (!ssInstalled)
                {
                    MethodInfo original = AccessTools.Method(typeof(Verb_ShootOneUse), "SelfConsume");
                    MethodInfo postfix = typeof(Harmony_Verb_ShootOneUse_SelfConsume_Postfix).GetMethod("SelfConsume");
                    if (GNATSettings.logSpam) Log.Message("GNAT_ShootOneUseInvCheck".Translate());
                    harmonyInstance.Patch(original: original, postfix: new HarmonyMethod(postfix));
                }
                else if (GNATSettings.logSpam) Log.Message("GNAT_ShootOneUseInvCheckSkip".Translate());
        }
    }
}
