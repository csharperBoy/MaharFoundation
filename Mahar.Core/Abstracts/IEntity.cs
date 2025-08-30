using System;

namespace Mahar.Core.Abstracts
{
    /// <summary>
    /// Marker interface for entities, used to constrain entity types by their identifier type.
    /// </summary>
    /// <typeparam name="TId">The type used for the entity identifier.</typeparam>
    public interface IEntity<TId>
    {
        // Marker interface - intentionally contains no members.
    }
}
