using Library.src.combat.Weapon;
using Library.src.io;
using Library.src.units;
using Library.src.util;
using Unity.Collections;
using Unity.Jobs;

namespace Library.src.combat
{
    public class Combat : ICombat, IResolvable
    {
        public bool mutual;
        
        NativeArray<float> dmgCache;
        JobHandle handle;

        readonly Broker broker;
        readonly UnitController unit;
        readonly UnitController opponent;

        CombatJob combatJob;

        bool ready;

        public Combat(Broker broker,
            UnitController unit,
            UnitController opponent,
            bool defending)
        {
            this.broker = broker;
            this.unit = unit;
            this.opponent = opponent;

            if (!defending && unit.unit.weapon.weaponType == WeaponType.Melee)
            {
                ready = false;
                var moveTo = Locator.GetNearest(
                    unit.gameObject.transform.position, 
                    opponent.gameObject);
                unit.MoveTo(moveTo.transform.position);
            }
            else if (unit.unit.weapon.weaponType == WeaponType.Ranged)
            {
                ready = true;
            }
            else if (defending && unit.unit.weapon.weaponType == WeaponType.Melee)
            {
                unit.WaitForAttacker();
            }
            else
            {
                ready = false;
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
            opponent.unit.health -= dmgCache[0];
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

        public UnitController GetUnit()
        {
            return unit;
        }

        public UnitController GetOpponent()
        {
            return opponent;
        }

        public void SetMutual(bool mutual)
        {
            this.mutual = mutual;
        }

        public void SetReady(bool ready)
        {
            this.ready = ready;
        }

        public void Resolve()
        {
            ResolveDamage();
        }
    }
}
