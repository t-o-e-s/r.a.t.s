using Library.src.time.records;

namespace Library.src.time
{
    public interface ITimeSensitive
    {
        bool SaveRecord();
        Record GetLastRecord();
        Record GetNextRecord();
        void ClearRecords();
    }
}