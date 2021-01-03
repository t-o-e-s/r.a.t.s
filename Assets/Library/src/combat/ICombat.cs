using Library.src.units;

namespace Library.src.combat
{
    public interface ICombat
    {
        Unit GetUnit();
        Unit GetOpponent();

        void SetMutual(bool mutual);
        void SetReady(bool ready);
        
    }
}
