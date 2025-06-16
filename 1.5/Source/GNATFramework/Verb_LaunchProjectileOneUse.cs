using System.Collections.Generic;
using Verse;

namespace GNATFramework
{
    public class Verb_LaunchProjectileOneUse : Verb_LaunchProjectile
    {
        protected override bool TryCastShot()
        {
            if (base.TryCastShot())
            {
                if (burstShotsLeft <= 1)
                {
                    SelfConsume();
                }
                return true;
            }
            if (burstShotsLeft < verbProps.burstShotCount)
            {
                SelfConsume();
            }
            return false;
        }

        public override void Notify_EquipmentLost()
        {
            base.Notify_EquipmentLost();
            if (state == VerbState.Bursting && burstShotsLeft < verbProps.burstShotCount)
            {
                SelfConsume();
            }
        }

        private void SelfConsume()
        {
            if (EquipmentSource != null && !EquipmentSource.Destroyed)
            {
                EquipmentSource.Destroy();
            }
            if (HarmonyInit.ssInstalled || 
                !(caster is Pawn pawn) || 
                pawn.equipment.GetDirectlyHeldThings().Any || 
                !(pawn.inventory?.innerContainer?.InnerListForReading is List<Thing> pawnInv)
                ) return;
            foreach (Thing thing in pawnInv)
                if (thing.def == EquipmentSource.def)
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
