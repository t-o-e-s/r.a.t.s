using Unity.Collections;
using Unity.Jobs;

namespace Library.Combat
{
    public class Combat : ICombat, IResolvable
    {
        NativeArray<float> dmgCache;
        JobHandle handle;

        readonly Broker broker;
        readonly UnitController attacker;
        readonly UnitController defender;

        CombatJob combatJob;
    

        public Combat(Broker broker,
            UnitController attacker,
            UnitController defender)
        {
            this.broker = broker;
            this.attacker = attacker;
            this.defender = defender;
        }
    
        public void ResolveDamage()
        { 
            combatJob = new CombatJob();
            handle = combatJob.Schedule();
        }

        public void CacheDamage()
        {
            handle.Complete();
        }

        public void DealDamage()
        {
            attacker.health -= dmgCache[0];
        }

        public void RefreshJob()
        {
            //TODO sort fields on unit controller and set them
            //combatJob.attackPower = attacker
            //combatJob.combatSpeed = attacker
            //combatJob.defense = defender
            //combatJob.combatSpeed = broker
            //combatJob.x = broker
            combatJob.result = dmgCache;
        }

        public void Resolve()
        {
            ResolveDamage();
        }
    }
}
