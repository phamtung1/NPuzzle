using System;
using System.Collections.Generic;
using System.Text;

namespace N_Puzzle
{
    class PriorityQueue
    {
        private SortedList<int, Queue<Node>> list;
        private HashSet<ulong> idList;
        public PriorityQueue()
        {
            list = new SortedList<int, Queue<Node>>();
            idList = new HashSet<ulong>();
        }

        public Node GetFirst()
        {
            var queue = list.Values[0];
            var matrix = queue.Dequeue();
            idList.Remove(matrix.Id);
            if (queue.Count == 0)
            {
                list.RemoveAt(0);
            }

            return matrix;
        }
        public ulong Max;
        public void Add(Node item)
        {
            if (idList.Contains(item.Id))
                return;

            idList.Add(item.Id);
            var comparingValue = item.HeuristicScore;
            if (item.Id > Max)
                Max = item.Id;
            if (list.ContainsKey(comparingValue))
            {
                list[comparingValue].Enqueue(item);
            }
            else
            {
                var queue = new Queue<Node>();
                queue.Enqueue(item);
                list.Add(comparingValue, queue);
            }
        }
        public void Clear()
        {
            list.Clear();
            idList.Clear();
        }

        public int Count
        { get { return list.Count; } }

    }
}
