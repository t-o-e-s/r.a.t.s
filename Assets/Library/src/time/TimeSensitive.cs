using System.Collections.Generic;
using Library.src.animation;
using Library.src.time.records;
using Library.src.units;
using UnityEngine;

namespace Library.src.time
{
    public abstract class TimeSensitive : MonoBehaviour
    {
        protected readonly List<Record> nextRecords = new List<Record>();
        protected readonly List<Record> previousRecords = new List<Record>();

        protected IAnimationController anim;

        protected Coroutine rewindRoutine;


        public abstract void Record();
        public abstract void PlayRecord(Record record);

        public void Forward()
        {
            var index = nextRecords.Count - 1;
            var record = nextRecords[index];
            nextRecords.RemoveAt(index);
            PlayRecord(record);

        }

        public void Rewind()
        {
            anim.Rewind(true);
            PlayRecord(PreviousRecord());
        }

        public Record RecordUnit(Unit unit)
        {
            return new UnitRecord(unit);
        }

        public bool IsRewinding()
        {
            return rewindRoutine != null;
        }

        protected Record PreviousRecord()
        {
            var index = previousRecords.Count - 1;
            var record = previousRecords[index];
            previousRecords.RemoveAt(index);
            return record;
        }

        public virtual void Stop()
        {
            StopCoroutine(rewindRoutine);
            nextRecords.Clear();
            previousRecords.Clear();
            rewindRoutine = null;
            anim.Rewind(false);
        }
    }
}