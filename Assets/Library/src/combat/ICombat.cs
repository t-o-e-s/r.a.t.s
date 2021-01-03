using Library.src.units;

namespace Library.src.combat
{
    public interface ICombat
    {
        Unit GetUnit();
        Unit GetOpponent();
    }
}
