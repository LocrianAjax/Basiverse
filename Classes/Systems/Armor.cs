using System;

namespace Basiverse{
    
    [Serializable]
    class Armor{ // Armor has a name, a value and a cost
        private string _name = "None";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _armorVal = 0;
        public int ArmorValue{ get {return _armorVal;} set {_armorVal = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}

        public Armor(string inName, int inVal, int inCost){
            _name = inName;
            _armorVal = inVal;
            _cost = inCost;
        }

        public Armor(){

        }
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}