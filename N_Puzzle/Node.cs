using System;
using System.Collections.Generic;
using System.Text;

namespace N_Puzzle
{
    public enum MoveDirection
    {
        UP = 1, LEFT = 2, DOWN = 3, RIGHT = 4
    }
    public class Node
    {
        public ulong Id { get; set; }
        public int Size { get; set; }
        public int Length { get; set; }

        public Node Parent { get; set; }
        private int[] _value;

        public int BlankPosition { get; set; }
        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                HeuristicScore = _score * 5 + StepCount;
            }
        }
        public int HeuristicScore { get; private set; }

        public int BlankValue { get; set; }
        public MoveDirection Direction { get; set; }
        public int StepCount { get; set; }

        public Node(int size)
        {
            this.Size = size;
            this.BlankValue = size * size;
            this.Length = this.BlankValue;
            Initialize();
        }
        public void Initialize()
        {
            this._value = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                _value[i] = i + 1;
            }
            BlankPosition = Length - 1;
        }
        internal void GetId()
        {
            this.Id = 0;
            ulong n = 1;
            for (int i = 0; i < Length - 1; i++)
            {
                if (_value[i] == BlankValue)
                    BlankPosition = i;
                this.Id += (ulong)_value[i] * n;
                n *= 10;
            }
        }

        public int[] Value
        {
            get { return _value; }
            set
            {
                this._value = value;

                GetId();
            }
        }

        public int this[int index]
        {
            get { return _value[index]; }
            set { _value[index] = value; }
        }
        public int this[int x, int y]
        {
            get { return _value[x * Size + y]; }
            set { _value[x * Size + y] = value; }
        }


        public Node Clone()
        {
            Node m = (Node)this.MemberwiseClone();
            m._value = (int[])this._value.Clone();
            return m;
        }

        public bool CanMoveUp
        {
            get { return BlankPosition > Size - 1; }
        }
        public bool CanMoveDown
        {
            get { return BlankPosition < Length - Size; }
        }
        public bool CanMoveLeft
        {
            get { return GameSolver.IndexCols[BlankPosition] > 0; }
        }
        public bool CanMoveRight
        {
            get { return GameSolver.IndexCols[BlankPosition] < Size - 1; }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Node m = obj as Node;
            if (m == null)
                return false;
            return this.Id == m.Id;
        }
    }
}
