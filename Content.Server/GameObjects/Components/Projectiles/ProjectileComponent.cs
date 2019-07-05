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


namespace Content.Server.GameObjects.Components.Projectiles
{
    public class ProjectileComponent : Component, ICollideSpecial, ICollideBehavior
    {
#pragma warning disable 649        
        [Dependency] private readonly IEntitySystemManager _entitySystemManager;
#pragma warning restore 649

        public override string Name => "Projectile";

        public bool IgnoreShooter = true;

        private EntityUid Shooter = EntityUid.Invalid;

        public Dictionary<DamageType, int> damages = new Dictionary<DamageType, int>();

        /// <summary>
        /// Function that makes the collision of this object ignore a specific entity so we don't collide with ourselves
        /// </summary>
        /// <param name="shooter"></param>
        public void IgnoreEntity(IEntity shooter)
        {
            Shooter = shooter.Uid;
        }

        /// <summary>
        /// Special collision override, can be used to give custom behaviors deciding when to collide
        /// </summary>
        /// <param name="collidedwith"></param>
        /// <returns></returns>
        bool ICollideSpecial.PreventCollide(ICollidable collidedwith)
        {
            if (IgnoreShooter && collidedwith.Owner.Uid == Shooter)
                return true;
            return false;
        }

        /// <summary>
        /// Applys the damage when our projectile collides with its victim
        /// </summary>
        /// <param name="collidedwith"></param>
        void ICollideBehavior.CollideWith(List<IEntity> collidedwith)
        {
            var collisionSystem = _entitySystemManager.GetEntitySystem<CollisionSystem>();
            collisionSystem.HandleCollision(Owner);
            foreach (var entity in collidedwith)
            {
                if (entity.TryGetComponent(out DamageableComponent damage))
                {
                    if (damages.Count > 0)
                    {
                        foreach(var damageSpecific in damages)
                        {
                            damage.TakeDamage(damageSpecific.Key, damageSpecific.Value);
                        }
                    } else {
                        damage.TakeDamage(DamageType.Brute, 5);
                    }
                }
                collisionSystem.HandleCollisionWith(Owner, entity);
                if (entity.TryGetComponent(out CameraRecoilComponent recoilComponent)
                    && Owner.TryGetComponent(out PhysicsComponent physicsComponent))
                {
                    var direction = physicsComponent.LinearVelocity.Normalized;
                    recoilComponent.Kick(direction);
                }
            }

            if (collidedwith.Count > 0)
            {
                Owner.Delete();
            }
        }
    }
}
