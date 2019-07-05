using System;
using System.Linq;
using JetBrains.Annotations;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.Interfaces.GameObjects;
using Content.Shared.GameObjects;

namespace Content.Server.GameObjects.EntitySystems
{

    public interface IOnCollide
    {
        void OnCollide(CollisionEventArgs args)
    }

    public class CollisionEventArgs : EventArgs
    {
        public IEntity Owner { get; set; }
    }

    public interface IOnCollideWith
    {
        void OnCollideWith(CollisionWithEventArgs args)
    }

    public class CollisionWithEventArgs : EventArgs
    {
        public IEntity Owner { get; set; }
        public IEntity Target { get; set; }
    }

    [UsedImplicitly]
    public sealed class CollisionSystem : EntitySystem
    {
        public void HandleCollision(IEntity owner)
        {
            var eventArgs = new CollisionEventArgs
            {
                Owner = owner
            };
            var collideActs = owner.GetAllComponents<IOnCollide>().ToList();

            foreach (var collideAct in collideActs)
            {
                collideAct.OnCollide(eventArgs);
            }
        }
        public void HandleCollisionWith(IEntity owner, IEntity target)
        {
            var eventArgs = new CollisionWithEventArgs
            {
                Owner = owner,
                Target = target
            };
            var collideWithActs = owner.GetAllComponents<IOnCollideWith>().ToList();

            foreach (var collideWithAct in collideWithActs)
            {
                collideWithAct.OnCollideWith(eventArgs);
            }
        }
    }
}