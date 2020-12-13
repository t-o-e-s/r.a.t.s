using Library.src.units;

namespace Library.Combat
{
    public interface ICombat
    {
        void ResolveDamage();
        void DealDamage();
        void RefreshJob();
        UnitController GetUnit();
        UnitController GetOpponent();
        void SetMutual(bool mutual);
        void SetReady(bool ready);
    }
}
