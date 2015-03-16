namespace DBUtils
{
    /// <summary>
    /// Abstract ID which also contains an ancestor ID for which
    /// this one is a descendent.
    /// </summary>
    /// <typeparam name="T">The type of the ancestor.</typeparam>
    public abstract class DescendantAbstractId<T> : AbstractId where T : AbstractId
    {
        /// <summary>
        /// The Parent ID, if there is one.
        /// </summary>
        public T ParentId { get; protected set; }

        /// <summary>
        /// Create a new descendent of the specified parent with the specified root.
        /// </summary>
        /// <param name="parentId">The parent ID.</param>
        /// <param name="rootId">The root ID.</param>
        protected DescendantAbstractId(T parentId, string rootId) : base(rootId)
        {
            this.ParentId = parentId;
        }

        /// <summary>
        /// Create a new descendent of the specified parent and a random ID.
        /// </summary>
        /// <param name="parentId">The parent ID.</param>
        protected DescendantAbstractId(T parentId)
        {
            this.ParentId = parentId;
        }

        /// <summary>
        /// Create a new descendent with the specified root ID.
        /// </summary>
        /// <param name="rootId">The root ID.</param>
        protected DescendantAbstractId(string rootId) : base(rootId) { }

        /// <summary>
        /// Create a new descendent with no parent and a random root.
        /// </summary>
        protected DescendantAbstractId() { } 

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (this.ParentId == null)
            {
                return this.RootId.GetHashCode();
            }

            unchecked
            {
                var hashCode = 43;

                hashCode = hashCode * 47 + this.ParentId.GetHashCode();
                hashCode = hashCode * 47 + this.RootId.GetHashCode();

                return hashCode;
            }
        }

        /// <summary>
        /// Provides a string format of the ID.
        /// </summary>
        /// <returns>The ID as a string.</returns>
        public override string ToString()
        {
            return this.RootId + (this.ParentId == null ? string.Empty : "_" + this.ParentId);
        }
    }
}
