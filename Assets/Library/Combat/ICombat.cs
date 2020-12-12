namespace Library.Combat
{
    public interface ICombat
    {
        void ResolveDamage();
        void CacheDamage();
        void DealDamage();
        void RefreshJob();
    }
}
