﻿using Content.Server.GameObjects.EntitySystems;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Server.GameObjects.Components.Destructible.Thresholds.Behaviors
{
    public interface IThresholdBehavior : IExposeData
    {
        /// <summary>
        ///     Executes this behavior.
        /// </summary>
        /// <param name="owner">The entity that owns this behavior.</param>
        /// <param name="system">
        ///     An instance of <see cref="DestructibleSystem"/> to pull dependencies
        ///     and other systems from.
        /// </param>
        void Execute(IEntity owner, DestructibleSystem system);
    }
}
