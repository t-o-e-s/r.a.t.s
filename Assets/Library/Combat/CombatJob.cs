using Unity.Collections;
using Unity.Jobs;

namespace Library.Combat
{
    public struct CombatJob : IJob
    {
        public NativeArray<float> result;
    
        public float attackPower;
        public float attackSpeed;
        public float combatSpeed;
        public float defense;
        public float x;
    
        public void Execute()
        {
            var temp = attackSpeed / 10;
            temp = attackPower * temp;
            temp = temp - (defense * x);
            temp = combatSpeed * temp;

            result[0] = temp;
        }
    }
}
