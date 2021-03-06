using System;
using System.Collections.Generic;
using System.Text;

namespace N_Puzzle
{
    class GameSolver
    {
        public static int[] IndexRows;
        public static int[] IndexCols;

        private int _size = 3;
        private Node _node;
        private int WIN_VALUE = 0;
        private PriorityQueue openQueue;

        private HashSet<ulong> visitedNodes;
        public Stack<MoveDirection> Solution { get; set; }

        public GameSolver()
        {
            openQueue = new PriorityQueue();
            visitedNodes = new HashSet<ulong>();

            Solution = new Stack<MoveDirection>();
        }
        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                _node = new Node(_size);

                int m = _size * _size;
                IndexRows = new int[m];
                IndexCols = new int[m];
                for (int i = 0; i < m; i++)
                {
                    IndexRows[i] = i / _size;
                    IndexCols[i] = i % _size;
                }
            }
        }
        public Node Node
        {
            get { return _node; }
            set
            {
                _node = value;
            }
        }
        public void Shuffle()
        {
            do
            {
                NodeHelper.Shuffle(this._node);
            }
            while (!CanSolve(this._node));
        }

        public bool CanSolve()
        {
            return CanSolve(this._node);
        }

        public bool CanSolve(Node node)
        {
            int value = 0;
            for (int i = 0; i < node.Length; i++)
            {
                int t = node[i];
                if (t > 1 && t < node.BlankValue)
                {
                    for (int m = i + 1; m < node.Length; m++)
                        if (node[m] < t)
                            value++;

                }
            }

            if (Size % 2 == 1)
            {
                return value % 2 == 0;
            }
            else
            {
                int row = IndexRows[_node.BlankPosition] + 1;
                return value % 2 == row % 2;
            }
        }
        public void Solve()
        {
            visitedNodes.Clear();
            openQueue.Clear();
            Solution.Clear();

            this._node.Parent = null;
            this._node.Score = EvaluateManhattanDistance(this._node);
            openQueue.Add(this._node);


            while (openQueue.Count > 0)
            {
                Node m = openQueue.GetFirst();

                if (m.Score == WIN_VALUE)
                {
                    TrackPath(m);
                    return;
                }

                GenerateMoves(m);
            }
        }

        private void TrackPath(Node node)
        {
            if (node.Parent != null)
            {
                Solution.Push(node.Direction);
                TrackPath(node.Parent);
            }
        }

        private void GenerateMoves(Node node)
        {
            if (visitedNodes.Contains(node.Id))
            {
                return;
            }
            else
                visitedNodes.Add(node.Id);

            if (node.Direction != MoveDirection.LEFT && node.CanMoveRight)
            {
                CloneMove(node, MoveDirection.RIGHT);
            }
            if (node.Direction != MoveDirection.UP && node.CanMoveDown)
            {
                CloneMove(node, MoveDirection.DOWN);
            }
            if (node.Direction != MoveDirection.RIGHT && node.CanMoveLeft)
            {
                CloneMove(node, MoveDirection.LEFT);
            }

            if (node.Direction != MoveDirection.DOWN && node.CanMoveUp)
            {
                CloneMove(node, MoveDirection.UP);
            }
        }

        private void CloneMove(Node parent, MoveDirection direction)
        {
            Node m = parent.Clone();
            NodeHelper.MakeMove(m, direction);
            m.Direction = direction;
            m.StepCount++;

            if (visitedNodes.Contains(m.Id))
            {
                return;
            }
            else
            {
                m.Parent = parent;
                m.Score = EvaluateManhattanDistance(m);
                openQueue.Add(m);
            }
        }

        private static int EvaluateManhattanDistance(Node node)
        {
            int score = 0;

            for (int i = 0; i < node.Length; i++)
            {
                int value = node[i] - 1;
                score += Math.Abs(IndexRows[i] - IndexRows[value]) + Math.Abs(IndexCols[i] - IndexCols[value]);
            }
            return score;
        }
    }
}
