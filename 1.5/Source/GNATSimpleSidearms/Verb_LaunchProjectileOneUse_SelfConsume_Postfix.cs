using GNATFramework;
using HarmonyLib;
using PeteTimesSix.SimpleSidearms.Utilities;
using PeteTimesSix.SimpleSidearms;
using SimpleSidearms.rimworld;
using Verse;

namespace GNATSimpleSidearms
{
    [HarmonyPatch(typeof(Verb_LaunchProjectileOneUse), "SelfConsume")]
    public static class Verb_LaunchProjectileOneUse_SelfConsume_Postfix
    {
        [HarmonyPostfix]
        public static void SelfConsume(Verb_LaunchProjectileOneUse __instance)
        {
            if (__instance.caster is Pawn pawn)
            {
                ThingDefStuffDefPair weapon = __instance.EquipmentSource.toThingDefStuffDefPair();
                if (!WeaponAssingment.equipSpecificWeaponTypeFromInventory(pawn, weapon, dropCurrent: false, intentionalDrop: false))
                {
                    WeaponAssingment.equipBestWeaponFromInventoryByPreference(pawn, Enums.DroppingModeEnum.UsedUp);
                }
            }
        }
    }
}
