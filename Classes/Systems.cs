using System;
using Basiverse;
using System.Collections.Generic;

namespace Basiverse
{
    [Serializable]
    class Hull{ // Hulls have a name, a current value, a max value and a heat max
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _hullval;
        public int Hullval{ get {return _hullval;} set {_hullval = value;}}
        private int _maxhull;
        public int HullMax{ get {return _maxhull;} set {_maxhull = value;}}

        private int _maxheat;
        public int HeatMax{ get {return _maxheat;} set {_maxheat = value;}}

        public double Health(){
            return (_hullval/_maxhull) * 100;
        }
    }

    [Serializable]
    class Armor{ // Armor has a name, a value and a cost
        private string _name = "None";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _armorVal = 0;
        public int ArmorValue{ get {return _armorVal;} set {_armorVal = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
    }

    [Serializable]
    class Heatsink{ // Heatsinks have a name, a pasive value, an active value, an active flag, and a cost
        private string _name = "None";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _passiveval = 0;
        public int PassiveVal{ get {return _passiveval;} set {_passiveval = value;}}

        private int _activeval = 0;
        public int ActiveVal{ get {return _activeval;} set {_activeval = value;}}

        private bool _isactive = false;
        public bool IsActive{ get {return _isactive;} set {_isactive = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
    }
    
    [Serializable]
    class Shield{ // Shield have a name, a value, a max value, an online flag and a cost
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _shieldval;
        public int ShieldVal{ get {return _shieldval;} set {_shieldval = value;}}
        private int _maxshield;
        public int ShieldMax{ get {return _maxshield;} set {_maxshield = value;}}
        private bool _isonline = true;
        public bool IsOnline{get {return _isonline;} set {_isonline = value;}}
        public double Health(){
            return (_shieldval/_maxshield) * 100;
        }

        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
    }

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
    }

    [Serializable]    
    class Missile{ // Missiles have a name, a hit chance, damage value, a stock value and a cost
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private int _damage;
        public int Damage{get {return _damage;} set {_damage = value;}}
        private double _hitchance;
        public double HitChance{get {return _hitchance;} set {_hitchance = value;}}
        private int _stock;
        public int Stock{get {return _stock;} set {_stock = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}

    }

    [Serializable]
    class Engine{ // Engines have a name a flee chance and a cost
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private double _fleechance;
        public double FleeChance{get {return _fleechance;} set {_fleechance = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
        
    }

    [Serializable]
    class CargoHold{ // Cargo Holds have a name, a max size, a current size and a list of cargo
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private int _maxSize = 0;
        public int MaxSize{ get {return _maxSize;} set {_maxSize = value;}}

        private int _currSize = 0;
        public int CurrentSize{ get {return _currSize;} set {_currSize = value;}}

        public List<Cargo> HoldItems = new List<Cargo>();
}
    
    [Serializable]
    class Cargo{ // Cargo has a name, a size in m3, and a cost
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private int _size = 0;
        public int Size{ get {return _size;} set {_size = value;}}

        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}