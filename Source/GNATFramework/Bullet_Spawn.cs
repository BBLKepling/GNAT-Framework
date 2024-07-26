using RimWorld;
using Verse;

namespace GNATFramework
{
    public class Bullet_Spawn : Bullet
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            Map map = Map;
            IntVec3 position = Position;
            base.Impact(hitThing, blockedByShield);
            if (def.projectile.preExplosionSpawnThingDef == null ||
                hitThing != null ||
                blockedByShield ||
                !position.IsValid ||
                def.projectile.preExplosionSpawnThingCount <= 0 ||
                !Rand.Chance(def.projectile.preExplosionSpawnChance))
                return;
            ThingDef thingDef = def.projectile.preExplosionSpawnThingDef;
            int count = def.projectile.preExplosionSpawnThingCount;
            if (thingDef.IsFilth && position.Walkable(map))
            {
                FilthMaker.TryMakeFilth(position, map, thingDef, count);
            }
            else if (GNATSettings.reuseNeoAmmo)
            {
                Thing thing = ThingMaker.MakeThing(thingDef);
                thing.stackCount = count;
                thing.SetForbidden(GNATSettings.forbidNeoAmmo, false);
                GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);
            }
        }
    }
}
