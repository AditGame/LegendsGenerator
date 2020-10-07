// -------------------------------------------------------------------------------------------------
// <copyright file="SortedKeylessCollection.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of SortedList which does not have a key (and thus allows duplicates).
    /// </summary>
    /// <typeparam name="TKey">The key, used for sorting.</typeparam>
    /// <typeparam name="TValue">The value, used for sorting.</typeparam>
    public class SortedKeylessCollection<TKey, TValue> : IEnumerable<TValue>, IEnumerable
        where TKey : notnull
    {
        /// <summary>
        /// The inner object of the list.
        /// </summary>
        private List<(TKey Key, TValue Obj)> inner = new List<(TKey, TValue)>();

        /// <summary>
        /// The internal comparer for comparing elements.
        /// </summary>
        private KeyComparer keyComparer = new KeyComparer();

        /// <summary>
        /// Gets the number of elements in this list.
        /// </summary>
        public int Count => this.inner.Count;

        /// <summary>
        /// Gets the comparer used to compare keys.
        /// </summary>
        public IComparer<TKey> Comparer => this.keyComparer.InnerComparer;

        /// <summary>
        /// Gets or sets the number of elements that the System.Collections.Generic.SortedList`2 can contain.
        /// </summary>
        public int Capacity
        {
            get => this.inner.Capacity;
            set => this.inner.Capacity = value;
        }

        /// <summary>
        /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList`2.
        /// </summary>
        /// <param name="key">The key of the element to add, used for sorting.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public void Add(TKey key, TValue value)
        {
            int index = this.inner.BinarySearch((key, value), this.keyComparer);
            if (index < 0)
            {
                // If BinarySerach does not find the right index, it returns the bitwise complement of the index which is the smallest larger value.
                index = ~index;
            }

            this.inner.Insert(index, (key, value));
        }

        /// <summary>
        /// Removes all elements from the System.Collections.Generic.SortedList`2.
        /// </summary>
        public void Clear()
        {
            this.inner.Clear();
        }

        /// <inheritdoc/>
        public IEnumerator<TValue> GetEnumerator()
        {
            return this.inner.Select(x => x.Obj).GetEnumerator();
        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="value">The element to remove.</param>
        /// <returns>True if removed, false if the element was not found.</returns>
        public bool Remove(TValue value)
        {
            return this.inner.RemoveAll(e => object.Equals(e.Obj, value)) != 0;
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the System.Collections.Generic.SortedList`2, if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess()
        {
            this.inner.TrimExcess();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Compares the keys of the class.
        /// </summary>
        private class KeyComparer : IComparer<(TKey Key, TValue Value)>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyComparer"/> class.
            /// </summary>
            public KeyComparer()
                : this(Comparer<TKey>.Default)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyComparer"/> class.
            /// </summary>
            /// <param name="inner">The comparer to use to compare key values.</param>
            public KeyComparer(IComparer<TKey> inner)
            {
                this.InnerComparer = inner;
            }

            /// <summary>
            /// Gets the inner comparer.
            /// </summary>
            public IComparer<TKey> InnerComparer { get; }

            /// <inheritdoc/>
            public int Compare((TKey Key, TValue Value) x, (TKey Key, TValue Value) y)
            {
                return this.InnerComparer.Compare(x.Key, x.Key);
            }
        }
    }
}
