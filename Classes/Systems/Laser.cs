using System;
using Basiverse;
using System.Collections.Generic;

namespace Basiverse{

    [Serializable]
    class Laser{ // Lasers have a name, a damage value, a heat value and a cost
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private int _damage;
        public int Damage{get {return _damage;} set {_damage = value;}}
        private int _heat;
        public int Heat{get {return _heat;} set {_heat = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}

        public Laser(string inName, int inDamage, int inHeat, int inCost){
            _name = inName;
            _damage = inDamage;
            _heat = inHeat;
            _cost = inCost;
        }

        public Laser(){

        }
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}