namespace Library.src.combat.Weapon
{
    public class Arsenal
    {
        public static Weapon Fists()
        {
            return new Weapon(0, "fists", 25f, 1f, 1f, WeaponType.Melee);
        }
    }
}
