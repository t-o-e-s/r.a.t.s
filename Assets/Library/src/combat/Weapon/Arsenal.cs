namespace Library.src.combat.Weapon
{
    public class Arsenal
    {
        public static Weapon Fists()
        {
            return new Weapon(0, "fists", 1, 1, 1, WeaponType.Melee);
        }
    }
}
