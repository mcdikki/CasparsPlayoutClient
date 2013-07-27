using System;
using System.Collections;
using System.Collections.Generic;

namespace Bespoke.Common
{
    /// <summary>
    /// A single-parented node forming a tree structure.
    /// </summary>
    /// <typeparam name="T">The type of data contained within the node.</typeparam>
    public class Node<T> : IEnumerable<T> where T: class
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent of the node.
        /// </summary>
        public Node<T> Parent
        {
            get
            {
                return mParent;
            }
            internal set
            {
                mParent = value;
            }
        }

        /// <summary>
        /// Gets the root of this node's branch of the tree structure.
        /// </summary>
        public Node<T> Root
        {
            get
            {
                if (mParent == null)
                {
                    return this;
                }
                else
                {
                    return mParent.Root;
                }
            }
        }

        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        public NodeCollection<T> Children
        {
            get
            {
                return mChildren;
            }
        }

        /// <summary>
        /// Gets the data associated with this node.
        /// </summary>
        public T Data
        {
            get
            {
                return mData;
            }
            set
            {
                mData = value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Node{T}"/> class.
        /// </summary>
        /// <param name="data">The data to associate with the node.</param>
        public Node(T data)
        {
            mChildren = new NodeCollection<T>(this);
            mData = data;
        }

        #region Public Methods

        /// <summary>
        /// Determines if the current node is an ancestor of the specified node.
        /// </summary>
        /// <param name="node">The node to test against.</param>
        /// <returns>true if the current node is an ancestore of <paramref name="node"/>; otherwise, false.</returns>
        public bool IsAncestorOf(Node<T> node)
        {
            if (mChildren.Contains(node))
            {
                return true;
            }

            foreach (Node<T> child in mChildren)
            {
                if (child.IsAncestorOf(node))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the current node is a descendant of the specified node.
        /// </summary>
        /// <param name="node">The node to test against.</param>
        /// <returns>true if the current node is an descendant of <paramref name="node"/>; otherwise, false.</returns>
        public bool IsDescendantOf(Node<T> node)
        {
            if (mParent == null)
            {
                return false;
            }

            if (node == mParent)
            {
                return true;
            }

            return mParent.IsDescendantOf(node);
        }

        /// <summary>
        /// Determines if the current node is an ancestor or descendant of the specified node.
        /// </summary>
        /// <param name="node">The node to test against.</param>
        /// <returns>true if the current node is an ancestore or descendant of <paramref name="node"/>; otherwise, false.</returns>
        public bool SharesHierarchyWith(Node<T> node)
        {
            if (node == this || IsAncestorOf(node) || IsDescendantOf(node))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Retreive a depth-first search enumerator for this node.
        /// </summary>
        /// <returns>A depth-first search enumerator for this node.</returns>
        public IEnumerator<T> GetDepthFirstEnumerator()
        {
            yield return mData;
            foreach (Node<T> child in mChildren)
            {
                IEnumerator<T> childEnumerator = child.GetDepthFirstEnumerator();
                while (childEnumerator.MoveNext())
                {
                    yield return childEnumerator.Current;
                }
            }
        }

        /// <summary>
        /// Retreive a breadth-first search enumerator for this node.
        /// </summary>
        /// <returns>A breadth-first search enumerator for this node.</returns>
        public IEnumerator<T> GetBreadthFirstEnumerator()
        {
            Queue<Node<T>> queue = new Queue<Node<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                Node<T> node = queue.Dequeue();
                foreach (Node<T> child in node.mChildren)
                {
                    queue.Enqueue(child);
                }

                yield return node.mData;
            }
        }

        /// <summary>
        /// Gets the default enumerator (breadth-first).
        /// </summary>
        /// <returns>A breadth-first search enumerator for this node.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return GetDepthFirstEnumerator();
        }

        /// <summary>
        /// Gets the default enumerator (breadth-first).
        /// </summary>
        /// <returns>A breadth-first search enumerator for this node.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetDepthFirstEnumerator();
        }

        #endregion

        private Node<T> mParent;
        private NodeCollection<T> mChildren;
        private T mData;
    }

    /// <summary>
    /// A collection of nodes, forming a tree structure.
    /// </summary>
    /// <typeparam name="T">The type of data contained within each node.</typeparam>
    public class NodeCollection<T> : IEnumerable<Node<T>> where T : class
    {
        #region Properties

        /// <summary>
        /// Gets the owner of this node collection.
        /// </summary>
        public Node<T> Owner
        {
            get
            {
                return mOwner;
            }
        }

        /// <summary>
        /// Gets the number of nodes contained within this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return mList.Count;
            }
        }

        /// <summary>
        /// Gets the node at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the node to get.</param>
        /// <returns>The node at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0. -or- index is equal to or greater than <see cref="Count"/>.</exception>
        public Node<T> this[int index]
        {
            get
            {
                return mList[index];
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCollection{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner of the collection.</param>
        public NodeCollection(Node<T> owner)
        {
            Assert.ParamIsNotNull(owner);

            mOwner = owner;
            mList = new List<Node<T>>();
        }

        #region Public Methods

        /// <summary>
        /// Add a node to the collection.
        /// </summary>
        /// <param name="item">The node to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if the node is already a member of the hierarchy.</exception>
        public void Add(Node<T> item)
        {
            if (mOwner.SharesHierarchyWith(item))
            {
                throw new InvalidOperationException("Cannot add a node that is already a member of the hierarchy.");
            }

            mList.Add(item);
            item.Parent = mOwner;
        }

        /// <summary>
        /// Remove a node from the collection.
        /// </summary>
        /// <param name="item">The node to remove.</param>
        public bool Remove(Node<T> item)
        {
            return mList.Remove(item);
        }

        /// <summary>
        /// Determines whether a node is in the collection.
        /// </summary>
        /// <param name="item">The node to locate.</param>
        /// <returns>true if node is found in the collection; otherwise, false.</returns>
        public bool Contains(Node<T> item)
        {
            return mList.Contains(item);
        }

        /// <summary>
        /// Removes all nodes from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (Node<T> node in this)
            {
                node.Parent = null;
            }

            mList.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<Node<T>> GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A enumerator for the collection.</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        #endregion

        private Node<T> mOwner;
        private List<Node<T>> mList;
    }
}
