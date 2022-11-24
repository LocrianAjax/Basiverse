using System;

namespace Basiverse{
    
    [Serializable]
    class Armor:BaseSystem{ // Armor has a name, a value and a cost
        private int _armorVal = 0;
        public int ArmorValue{ get {return _armorVal;} set {_armorVal = value;}}

        public Armor(string inName, int inVal, int inCost){
            Name = inName;
            Cost = inCost;
            _armorVal = inVal;
        }

        public Armor(){

        }
    }
}