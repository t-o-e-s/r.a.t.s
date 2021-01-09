using Library.src.units;
using Library.src.util;

namespace Library.src.combat
{
    public class Combat : ICombat, IResolvable
    {
        readonly Broker broker;
        
        //this unit
        Unit friendlyUnit;
        //target unit (to attack)
        Unit targetUnit;

        bool mutual = false;
        bool ready = false;
        
        public Combat(Broker broker,
            Unit friendlyUnit,
            Unit targetUnit)
        {
            this.broker = broker;
            this.friendlyUnit = friendlyUnit;
            this.targetUnit = targetUnit;
        }
        
        public void Resolve()
        {
            //TODO add check if the combat 
            DealDamage();
        }

        public void DealDamage()
        {
            //TODO calculate damage
            var damage = 1f;
            targetUnit.health -= damage;
        }

        public Unit GetUnit()
        {
            return friendlyUnit;
        }

        public Unit GetOpponent()
        {
            return targetUnit;
        }

        public void SetMutual(bool mutual)
        {
            this.mutual = mutual;
        }

        public void SetReady(bool ready)
        {
            this.ready = ready;
        }
    }
}
