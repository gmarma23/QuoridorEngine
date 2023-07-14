using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class TranspositionTable
    {
        private struct EntryType
        {
            public bool Valid { get; set; }
            public float Evaluation { get; set; }

            public EntryType()
            {
                Valid = false;
                Evaluation = 0;
            }
        }

        private readonly EntryType[] table;

        public int Capacity { get; private init; }
        public int Count { get; private set; }

        public TranspositionTable(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Invalid Transposition Table capacity!");

            table = new EntryType[capacity];
            Capacity = capacity;

            Clear();
        }

        public void Add(long key, float evaluation)
        {
            int hashedKey = hash(key);
            table[hashedKey].Evaluation = evaluation;
            table[hashedKey].Valid = true;
            Count++;
        }

        public float Get(long key)
        {
            Debug.Assert(HasKey(key));
            return table[hash(key)].Evaluation;
        }

        public bool HasKey(long key)
        {
            int hashedKey = hash(key);
            return table[hashedKey].Valid;
        }

        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
                table[i].Valid = false;
            Count = 0;
        }

        private int hash(long key) => (int)(key % Capacity);
    }
}
