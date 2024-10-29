using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace GNATFramework
{
    [HarmonyPatch(typeof(PawnWeaponGenerator), nameof(TryGenerateWeaponFor))]
    class Harmony_PawnWeaponGenerator_TryGenerateWeaponFor_Postfix
    {
        [HarmonyPostfix]
        public static void TryGenerateWeaponFor(Pawn pawn)
        {
            if (pawn?.inventory?.innerContainer is null || 
                !(pawn.equipment?.Primary?.def?.GetModExtension<GenerateWithEquip>()?.generateEquip is List<ThingDefCountRangeClass> generateThis)
                ) return;
            foreach (ThingDefCountRangeClass item in generateThis)
            {
                Thing thing = ThingMaker.MakeThing(item.thingDef, GenStuff.AllowedStuffsFor(item.thingDef).Any() ? pawn.equipment.Primary.Stuff ?? GenStuff.AllowedStuffsFor(item.thingDef).RandomElement() : null);
                thing.stackCount = item.countRange.RandomInRange;
                pawn.inventory.innerContainer.TryAdd(thing);
            }
        }
    }
    [HarmonyPatch(typeof(Verb_ShootOneUse), nameof(SelfConsume))]
    public static class Harmony_Verb_ShootOneUse_SelfConsume_Postfix
    {
        [HarmonyPostfix]
        public static void SelfConsume(Verb_ShootOneUse __instance)
        {
            if (!(__instance.caster is Pawn pawn) || 
                pawn.equipment.GetDirectlyHeldThings().Any || 
                !(pawn.inventory?.innerContainer?.InnerListForReading is List<Thing> pawnInv)
                ) return;
            foreach (Thing thing in pawnInv)
                if (thing.def == __instance.EquipmentSource.def)
                {
                    pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.equipment.GetDirectlyHeldThings(), 1, false);
                    return;
                }
            foreach (Thing thing in pawnInv)
                if (thing.def.IsRangedWeapon)
                {
                    pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.equipment.GetDirectlyHeldThings(), 1, false);
                    return;
                }
            foreach (Thing thing in pawnInv)
                if (thing.def.IsWeapon)
                {
                    pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.equipment.GetDirectlyHeldThings(), 1, false);
                    return;
                }
        }
    }
}
