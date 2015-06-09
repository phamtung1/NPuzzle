using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_Puzzle
{
    class NodeHelper
    {
        public static void Shuffle(Node node)
        {
            // matrix.Value = new int[]{6,9,11,2,3,5,6,16,12,13,8,10,15,14,4,1}; // forever
            var random = new Random();
            var length = node.Length;
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(length);

                if (i != index)
                {
                    int temp = node[i];
                    node[i] = node[index];
                    node[index] = temp;

                    if (node[i] == node.BlankValue)
                    {
                        node.BlankPosition = i;
                    }
                    else if (node[index] == node.BlankValue)
                    {
                        node.BlankPosition = index;
                    }
                }
            }

            node.GetId();
        }

        public static void MakeMove(Node node, MoveDirection direction)
        {
            int position;
            if (direction == MoveDirection.UP)
                position = node.BlankPosition - node.Size;
            else if (direction == MoveDirection.DOWN)
                position = node.BlankPosition + node.Size;
            else if (direction == MoveDirection.LEFT)
                position = node.BlankPosition - 1;
            else// if (direction == MoveDirection.RIGHT)
                position = node.BlankPosition + 1;

            node[node.BlankPosition] = node[position];
            node[position] = node.BlankValue;

            node.BlankPosition = position;
            node.GetId();
        }

    }
}
