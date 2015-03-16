using System;

namespace DBUtils
{
    /// <summary>
    /// Base class for all IDs.
    /// </summary>
    public abstract class AbstractId
    {
        /// <summary>
        /// Allow implementers read access to the root ID.
        /// </summary>
        public string RootId { get; protected set; }

        /// <summary>
        /// Create a new Abstract ID with the specified root.
        /// </summary>
        /// <param name="rootId">The root ID.</param>
        protected AbstractId(string rootId)
        {
            this.RootId = rootId;
        }

        /// <summary>
        /// Create a new Abstract ID with a random root.
        /// </summary>
        protected AbstractId()
        {
            this.RootId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Equals comparison.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the objects are the equal.</returns>
        public override bool Equals(object obj)
        {
            var targetId = obj as AbstractId;

            return targetId != null && this.GetHashCode().Equals(targetId.GetHashCode());
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.RootId.GetHashCode();
        }

        /// <summary>
        /// Provides a string format of the ID.
        /// </summary>
        /// <returns>The ID as a string.</returns>
        public override string ToString()
        {
            return this.RootId;
        }
    }
}
