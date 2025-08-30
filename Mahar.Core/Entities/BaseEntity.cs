using System;
using Mahar.Core.Abstracts;

namespace Mahar.Core.Entities
{
    /// <summary>
    /// Base abstract entity providing common audit properties.
    /// </summary>
    /// <typeparam name="TId">The type used for the entity identifier.</typeparam>
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
    /// <summary>
    /// The primary identifier for this entity.
    /// </summary>
    public TId Id { get; set; } = default!;

        /// <summary>
        /// The UTC date and time when the entity was created. Initialized in the constructor.
        /// </summary>
        public DateTime CreatedAtUtc { get; init; }

        /// <summary>
        /// Optional identifier of the user who created the entity.
        /// </summary>
        public string? CreatedByUserId { get; set; }

        /// <summary>
        /// The UTC date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedAtUtc { get; set; }

        /// <summary>
        /// Optional identifier of the user who last updated the entity.
        /// </summary>
        public string? UpdatedByUserId { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseEntity{TId}"/>, setting CreatedAtUtc and UpdatedAtUtc to DateTime.UtcNow.
        /// </summary>
        protected BaseEntity()
        {
            var now = DateTime.UtcNow;
            CreatedAtUtc = now;
            UpdatedAtUtc = now;
        }
    }

    /// <summary>
    /// Non-generic convenience base entity that uses an <see cref="int"/> identifier.
    /// </summary>
    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
