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
        static HarmonyInit()
        {
            List<string> features = new List<string>();
            foreach (ModFeatureDef def in DefDatabase<ModFeatureDef>.AllDefs)
            {
                foreach (string feature in def.features)
                {
                    features.Add(feature.ToLower());
                }
            }
            if (features.NullOrEmpty()) return;
            Harmony harmonyInstance = new Harmony("BBLKepling.GNAT");
            if (features.Contains("generatewithequip"))
            {
                MethodInfo original = AccessTools.Method(typeof(PawnInventoryGenerator), nameof(PawnInventoryGenerator.GenerateInventoryFor));
                MethodInfo postfix = typeof(Harmony_PawnInventoryGenerator_GenerateInventoryFor_Postfix).GetMethod("GenerateInventoryFor");
                Log.Message("[GNAT]Running GenerateWithEquip Patch");
                harmonyInstance.Patch(original: original, postfix: new HarmonyMethod(postfix));
            }
            if (features.Contains("shootoneuseinvcheck"))
                if (!ModLister.HasActiveModWithName("Simple sidearms"))
                {
                    MethodInfo original = AccessTools.Method(typeof(Verb_ShootOneUse), "SelfConsume");
                    MethodInfo postfix = typeof(Harmony_Verb_ShootOneUse_SelfConsume_Postfix).GetMethod("SelfConsume");
                    Log.Message("[GNAT]Running ShootOneUseInvCheck Patch");
                    harmonyInstance.Patch(original: original, postfix: new HarmonyMethod(postfix));
                }
                else Log.Message("[GNAT]Simple Sidearms detected, skipping ShootOneUseInvCheck Patch");
        }
    }
}
