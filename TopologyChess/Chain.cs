using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TopologyChess
{
    public class Chain<T> : IEnumerable<T>
    {
        public T Value { get; set; }
        public Chain<T> Previous { get; set; }

        public Chain() { }
        public Chain(T value)
        {
            Value = value;
            Previous = null;
        }

        public Chain<T> Add(T value) => new Chain<T> { Value = value, Previous = this };

        public LinkedList<T> ToLinkedList()
        {
            LinkedList<T> llist = new LinkedList<T>();
            Chain<T> elem = this;
            do
            {
                llist.AddFirst(elem.Value);
                elem = elem.Previous;
            } while (elem != null);
            return llist;
        }

        public IEnumerator<T> GetEnumerator() => ToLinkedList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToLinkedList().GetEnumerator();
    }
}
