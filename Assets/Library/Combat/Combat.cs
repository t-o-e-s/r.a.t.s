using Unity.Collections;
using Unity.Jobs;

namespace Library.Combat
{
    public class Combat : ICombat, IResolvable
    {
        NativeArray<float> dmgCache;
        JobHandle handle;

        readonly Broker broker;
        readonly UnitControllerController attacker;
        readonly UnitControllerController defender;

        CombatJob combatJob;

        bool ready;
    

        public Combat(Broker broker,
            UnitControllerController attacker,
            UnitControllerController defender)
        {
            this.broker = broker;
            this.attacker = attacker;
            this.defender = defender;

            if (weapon.GetWeaponType() == WeaponType.Melee)
            {
                ready = false;
                var moveTo = Locator.GetNearest(attacker.transform.position, defender.gameObject);
                attacker.Move(moveTo.transform.position);
            }
            else
            {
                ready = true;
            }
        }
    
        public void ResolveDamage()
        { 
            if (!ready) return;
            combatJob = new CombatJob();
            handle = combatJob.Schedule();
        }

        public void DealDamage()
        {
            handle.Complete();
            if (!ready) return;
            defender.health = defender.health - dmgCache[0];
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
