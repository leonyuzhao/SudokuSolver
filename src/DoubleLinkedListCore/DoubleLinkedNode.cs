using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleLinkedList
{
    public class DoubleLinkedNode<T>
    {
        public DoubleLinkedNode<T> Previous { get; set; }
        public DoubleLinkedNode<T> Next { get; set; }
        public T Data { get; set; }

        public DoubleLinkedNode() { }

        public DoubleLinkedNode(T data)
        {
            this.Data = data;
        }

        public DoubleLinkedNode(DoubleLinkedNode<T> previous, DoubleLinkedNode<T> next, T data)
        {
            this.Previous = previous;
            this.Next = next;
            this.Data = data;
        }
    }
}
