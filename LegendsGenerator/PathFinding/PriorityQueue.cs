// -------------------------------------------------------------------------------------------------
// <copyright file="PriorityQueue.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
//
// <remarks>
//     Whole or part copied from https://github.com/valantonini/AStar/blob/master/AStar.Core/PriorityQueueB.cs. See LICENSE.txt in this directory for details.
// </remarks>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System.Collections.Generic;

    /// <summary>
    /// A queue which changes order based on the priority of the inputted item.
    /// </summary>
    /// <typeparam name="T">The type of item in the queue.</typeparam>
    internal class PriorityQueue<T>
    {
        /// <summary>
        /// The underlying datastructure to this queue.
        /// </summary>
        private readonly List<T> innerList = new List<T>();

        /// <summary>
        /// The comparator to use to eval what item is higher priority.
        /// </summary>
        private readonly IComparer<T> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class.
        /// </summary>
        public PriorityQueue()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use to decide which elements have greater priority.</param>
        public PriorityQueue(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Gets the count of items in this list.
        /// </summary>
        public int Count => this.innerList.Count;

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item.</returns>
        public T this[int index]
        {
            get
            {
                return this.innerList[index];
            }

            set
            {
                this.innerList[index] = value;
                this.Update(index);
            }
        }

        /// <summary>
        /// Gets the next element without removing it from the queue.
        /// </summary>
        /// <returns>The next element, if it exists.</returns>
        public T? Peek()
        {
            return this.innerList.Count > 0 ? this.innerList[0] : default(T);
        }

        /// <summary>
        /// Clears the queue of all items.
        /// </summary>
        public void Clear()
        {
            this.innerList.Clear();
        }

        /// <summary>
        /// Pushes a new item into this queue. It's position will be determined by the Comparer.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        /// <returns>The item's position.</returns>
        public int Push(T item)
        {
            var p = this.innerList.Count;
            this.innerList.Add(item); // E[p] = O

            do
            {
                if (p == 0)
                {
                    break;
                }

                var p2 = (p - 1) / 2;

                if (this.OnCompare(p, p2) < 0)
                {
                    this.SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break;
                }
            }
            while (true);

            return p;
        }

        /// <summary>
        /// Removes the first item from the queue and returns it.
        /// </summary>
        /// <returns>The first item.</returns>
        public T Pop()
        {
            var result = this.innerList[0];
            var p = 0;

            this.innerList[0] = this.innerList[this.innerList.Count - 1];
            this.innerList.RemoveAt(this.innerList.Count - 1);

            do
            {
                var pn = p;
                var p1 = (2 * p) + 1;
                var p2 = (2 * p) + 2;

                if (this.innerList.Count > p1 && this.OnCompare(p, p1) > 0)
                {
                    p = p1;
                }

                if (this.innerList.Count > p2 && this.OnCompare(p, p2) > 0)
                {
                    p = p2;
                }

                if (p == pn)
                {
                    break;
                }

                this.SwitchElements(p, pn);
            }
            while (true);

            return result;
        }

        /// <summary>
        /// Update the priority queue after a new item is added.
        /// </summary>
        /// <param name="i">The added index.</param>
        private void Update(int i)
        {
            var p = i;
            int p2;

            do
            {
                if (p == 0)
                {
                    break;
                }

                p2 = (p - 1) / 2;

                if (this.OnCompare(p, p2) < 0)
                {
                    this.SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break;
                }
            }
            while (true);

            if (p < i)
            {
                return;
            }

            do
            {
                var pn = p;
                var p1 = (2 * p) + 1;
                p2 = (2 * p) + 2;

                if (this.innerList.Count > p1 && this.OnCompare(p, p1) > 0)
                {
                    p = p1;
                }

                if (this.innerList.Count > p2 && this.OnCompare(p, p2) > 0)
                {
                    p = p2;
                }

                if (p == pn)
                {
                    break;
                }

                this.SwitchElements(p, pn);
            }
            while (true);
        }

        /// <summary>
        /// Swap two elements.
        /// </summary>
        /// <param name="i">The first element.</param>
        /// <param name="j">The second element.</param>
        private void SwitchElements(int i, int j)
        {
            var h = this.innerList[i];
            this.innerList[i] = this.innerList[j];
            this.innerList[j] = h;
        }

        /// <summary>
        /// Compares two elements.
        /// </summary>
        /// <param name="i">The first element.</param>
        /// <param name="j">the second element.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y, as shown in the
        ///     following table.
        ///     Value – Meaning
        ///     Less than zero –x is less than y.
        ///     Zero –x equals y.
        ///     Greater than zero –x is greater than y.
        /// </returns>
        private int OnCompare(int i, int j)
        {
            return this.comparer.Compare(this.innerList[i], this.innerList[j]);
        }
    }
}
