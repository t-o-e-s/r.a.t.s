using Library.Units;
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
        readonly Unit attUnit;
        readonly Unit defUnit;

        CombatJob combatJob;

        bool ready;
    

        public Combat(Broker broker,
            UnitController attacker,
            UnitController defender)
        {
            this.broker = broker;
            this.attacker = attacker;
            this.defender = defender;
            attUnit = attacker.GetUnit();
            defUnit = defender.GetUnit();

            if (attUnit.weapon.weaponType == WeaponType.Melee)
            {
                ready = false;
                var moveTo = Locator.GetNearest(
                    attacker.gameObject.transform.position, 
                    defender.gameObject);
                attacker.MoveTo(moveTo.transform.position);
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
            defender.unit.health = defender.unit.health - dmgCache[0];
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
