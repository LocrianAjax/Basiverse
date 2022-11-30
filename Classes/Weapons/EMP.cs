using System;

namespace Basiverse{

    [Serializable]
    class EMP:Weapon{ // EMPs have a name, a damage value, and a cost

        public EMP(string inName, int inDamage, int inCost){ // They don't produce heat like lasers, but deal damage to only shields
            Name = inName;
            Damage = inDamage;
            Cost = inCost;
        }

        public EMP(){

        }
    }
}