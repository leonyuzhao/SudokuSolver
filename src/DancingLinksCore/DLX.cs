using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingLinks
{
    public class DLX
    {
        private DLXColumn i_Head; // Column 0 = Head
        private List<DLXColumn> i_Columns;
        private int i_CurrentRowNum;

        private Stack<int> i_Solution;

        public DLX(int columnNum)
        {
            if (columnNum <= 0) { throw new ArgumentOutOfRangeException(); }

            i_Columns = new List<DLXColumn>();
            i_CurrentRowNum = 0;
            i_Solution = new Stack<int>();

            // Initial Head
            i_Head = new DLXColumn();
            i_Head.Up = i_Head.Down = i_Head; // Left, Right leave till initial columns
            i_Head.Column = i_Head.Row = 0;
            i_Head.Count = 0;
            i_Columns.Add(i_Head);

            // Initial Columns
            DLXColumn currentNode = i_Head;
            DLXColumn rightNode = null;
            for (int i = 1; i < columnNum + 1; i++)
            {
                rightNode = new DLXColumn();
                currentNode.Right = rightNode;
                rightNode.Left = currentNode;
                // Up, Down leave till append line
                currentNode = rightNode;
                rightNode.Column = i;
                rightNode.Row = i_CurrentRowNum;
                rightNode.Count = 0;
                i_Columns.Add(rightNode);
            }
            // Close the circle
            i_Head.Left = rightNode;
            rightNode.Right = i_Head;
        }

        public void AppendRow(params int[] list)
        {
            if (list.Length == 0) { return; }

            i_CurrentRowNum++;

            DLXNode firstNode = new DLXNode();
            // Calculate the up/down link
            DLXNode temp = i_Columns[list[0]];
            while (temp.Down != null)
            {
                temp = temp.Down;
                if (temp.Down.Row == 0) { break; }
            }
            firstNode.Up = temp;
            temp.Down = firstNode;
            i_Columns[list[0]].Up = firstNode;

            firstNode.Down = i_Columns[list[0]];
            firstNode.Row = i_CurrentRowNum;
            firstNode.Column = list[0];
            i_Columns[list[0]].Count++;

            DLXNode tempNode = firstNode;
            DLXNode nextNode = null;
            for (int i = 1; i < list.Length; i++)
            {
                nextNode = new DLXNode();
                tempNode.Right = nextNode;
                nextNode.Left = tempNode;

                // Calculate the up/down link
                DLXNode nextTemp = i_Columns[list[i]];
                while (nextTemp.Down != null)
                {
                    nextTemp = nextTemp.Down;
                    if (nextTemp.Down.Row == 0) { break; }
                }
                nextNode.Up = nextTemp;
                nextTemp.Down = nextNode;
                i_Columns[list[i]].Up = nextNode;

                tempNode = nextNode;

                nextNode.Down = i_Columns[list[i]];
                nextNode.Row = i_CurrentRowNum;
                nextNode.Column = list[i];
                i_Columns[list[i]].Count++;
            }
            // Close the circle
            firstNode.Left = nextNode;
            nextNode.Right = firstNode;
        }

        public bool Solve(int depth)
        {
            if (i_Head.Right == i_Head)
            {
                return true;
            }
            DLXColumn columnNode = ChooseNextColumn();
            Cover(columnNode);

            DLXNode tempDownNode = columnNode.Down;
            while (tempDownNode != columnNode)
            {
                i_Solution.Push(tempDownNode.Row);

                DLXNode tempRightNode = tempDownNode.Right;
                while (tempRightNode != tempDownNode)
                {
                    Cover(i_Columns[tempRightNode.Column]);
                    tempRightNode = tempRightNode.Right;
                }

                if (Solve(depth + 1)) { return true; }

                i_Solution.Pop();

                DLXNode tempLeftNode = tempDownNode.Left;
                while (tempLeftNode != tempDownNode)
                {
                    Uncover(i_Columns[tempLeftNode.Column]);
                    tempLeftNode = tempLeftNode.Left;
                }

                tempDownNode = tempDownNode.Down;
            }

            Uncover(columnNode);
            return false;
        }

        private void Cover(DLXColumn column)
        {
            column.Right.Left = column.Left;
            column.Left.Right = column.Right;

            DLXNode tempDownNode = column.Down;
            while (tempDownNode != column)
            {
                DLXNode tempRightNode = tempDownNode.Right;
                while (tempRightNode != tempDownNode)
                {
                    tempRightNode.Down.Up = tempRightNode.Up;
                    tempRightNode.Up.Down = tempRightNode.Down;
                    tempRightNode = tempRightNode.Right;
                }
                tempDownNode = tempDownNode.Down;
            }
        }

        private void Uncover(DLXColumn column)
        {
            column.Right.Left = column;
            column.Left.Right = column;

            DLXNode tempUpNode = column.Up;
            while (tempUpNode != column)
            {
                DLXNode tempRightNode = tempUpNode.Right;
                while (tempRightNode != tempUpNode)
                {
                    tempRightNode.Down.Up = tempRightNode;
                    tempRightNode.Up.Down = tempRightNode;
                    tempRightNode = tempRightNode.Right;
                }
                tempUpNode = tempUpNode.Up;
            }
        }

        private DLXColumn ChooseNextColumn()
        {
            // Choose least count number column
            DLXColumn targetNode = null;
            DLXColumn tempNode = (DLXColumn)i_Head.Right;
            while (tempNode != i_Head)
            {
                if (targetNode == null || targetNode.Count > tempNode.Count)
                    targetNode = tempNode;
                tempNode = (DLXColumn)tempNode.Right;
            }
            return targetNode;
        }

        public Stack<int> Solution
        {
            get { return i_Solution; }
        }
    }
}