using System;


namespace Basiverse{

    [Serializable] 
    class Weapon{ // Contains all the shared components of weapons
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        
        private int _damage;
        public int Damage{get {return _damage;} set {_damage = value;}}

        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
    }
}


    /* Future Ideas: 
        "EMP Weapon" that only deals sheild damage
        Cloaking Device
        Weapons Jammer
    */
