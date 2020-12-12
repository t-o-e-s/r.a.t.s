namespace Library.Combat.Weapon
{
    public struct Weapon
    {
        public readonly int id;
        public readonly string name;
        public readonly float damage;
        public readonly float range;
        public readonly float speed;
        public readonly WeaponType weaponType;

        public Weapon(int id,
            string name,
            float damage,
            float range,
            float speed,
            WeaponType weaponType)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;
            this.range = range;
            this.speed = speed;
            this.weaponType = weaponType;
        } 

        public int GetID()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public WeaponType GetWeaponType()
        {
            return weaponType;
        }
    }
}
