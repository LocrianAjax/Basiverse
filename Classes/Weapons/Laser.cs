using System;
using Basiverse;
using System.Collections.Generic;

namespace Basiverse{

    [Serializable]
    class Laser:Weapon{ // Lasers have a name, a damage value, a heat value and a cost
        private int _heat;
        public int Heat{get {return _heat;} set {_heat = value;}}

        public Laser(string inName, int inDamage, int inHeat, int inCost){
            Name = inName;
            Damage = inDamage;
            _heat = inHeat;
            Cost = inCost;
        }

        public Laser(){

        }
    }
}