using System;

namespace Library.src.time.records
{
    public abstract class Record
    {
        public readonly int id;
        public readonly string name;

        public Record(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}