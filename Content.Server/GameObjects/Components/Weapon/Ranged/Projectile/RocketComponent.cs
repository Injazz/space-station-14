using System.Collections.Generic;
using Robust.Server.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Physics;
using Robust.Shared.Interfaces.GameObjects.Components;
using Content.Server.GameObjects.Components.Mobs;
using Content.Server.GameObjects.EntitySystems;
using Content.Shared.GameObjects;
using Content.Server.GameObjects.Components.Explosive;

namespace Content.Server.GameObjects.Components.Weapon.Ranged.Projectile
{
    public class RocketComponent : Component, IOnCollide
    {
        public override string Name => "Rocket";

        void IOnCollide.OnCollide(CollisionEventArgs args)
        {
            Owner.GetComponent<ExplosiveComponent>().Explosion();
        }
    }
}
