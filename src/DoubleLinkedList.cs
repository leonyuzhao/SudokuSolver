using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleLinkedList
{
    public class DoubleLinkedList<T>
    {
        private DoubleLinkedNode<T> i_head;
        private DoubleLinkedNode<T> i_tail;
        private int i_count;

        public DoubleLinkedList()
        {
            // Initial Double Linked List
            i_tail = i_head;
        }

        public void Insert(int position, T data)
        {
            // Empty Double Linked List 
            if (IsEmpty())
            {
                // Here won't consider pass in position value
                i_tail = i_head = new DoubleLinkedNode<T>(data);
                i_count++;
                return;
            }

            // Append mode
            if (position == i_count + 1)
            {
                i_tail.Next = new DoubleLinkedNode<T>(i_tail, null, data);
                i_tail = i_tail.Next;
                i_count++;
                return;
            }

            DoubleLinkedNode<T> oldNode = GetNode(position - 1);
            if (oldNode.Previous == null)
            {
                // new head
                i_head = new DoubleLinkedNode<T>(oldNode.Previous, oldNode, data);
                oldNode.Previous = i_head;
                i_count++;
                return;
            }

            DoubleLinkedNode<T> newNode = new DoubleLinkedNode<T>(oldNode.Previous, oldNode, data);
            oldNode.Previous.Next = newNode;
            oldNode.Previous = newNode;
            i_count++;
        }

        public void Delete(int position)
        {
            DoubleLinkedNode<T> oldNode = GetNode(position - 1);
            // new head
            if (oldNode.Previous == null)
            {
                i_head = oldNode.Next;
                i_head.Previous = null;
                i_count--;
                return;
            }
            // new tail
            if (oldNode.Next == null)
            {
                i_tail = oldNode.Previous;
                i_tail.Next = null;
                i_count--;
                return;
            }
            oldNode.Next.Previous = oldNode.Previous;
            oldNode.Previous.Next = oldNode.Next;
            i_count--;
        }

        public void Append(T data)
        {
            Insert(i_count + 1, data);
        }

        public bool IsEmpty()
        {
            return (i_head == null);
        }

        public override string ToString()
        {
            DoubleLinkedNode<T> temp = i_head;
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool isFirst = true;
            while (temp != null)
            {
                if (!isFirst) { sb.Append(","); }
                sb.Append(string.Format("{{{0}}}", temp.Data.ToString()));
                temp = temp.Next;
                isFirst = false;
            }
            sb.Append("}");
            return sb.ToString();
        }

        public string ToReverseString()
        {
            DoubleLinkedNode<T> temp = i_tail;
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool isFirst = true;
            while (temp != null)
            {
                if (!isFirst) { sb.Append(","); }
                sb.Append(string.Format("{{{0}}}", temp.Data.ToString()));
                temp = temp.Previous;
                isFirst = false;
            }
            sb.Append("}");
            return sb.ToString();
        }

        private DoubleLinkedNode<T> GetNode(int index)
        {
            if (index < 0 || index >= i_count) { throw new ArgumentOutOfRangeException(); }
            DoubleLinkedNode<T> currentNode;
            if (index < i_count / 2)
            {
                // Forwards
                currentNode = i_head;
                int i = 0;
                while (i < index)
                {
                    currentNode = currentNode.Next;
                    i++;
                }
            }
            else
            {
                // Backwards
                currentNode = i_tail;
                int i = i_count - 1;
                while (i > index)
                {
                    currentNode = currentNode.Previous;
                    i--;
                }
            }
            return currentNode;
        }

        public T this[int index]
        {
            get
            {
                return GetNode(index).Data;
            }
            set
            {
                GetNode(index).Data = value;
            }
        }

        public int Count
        {
            get { return i_count; }
        }
    }
}
