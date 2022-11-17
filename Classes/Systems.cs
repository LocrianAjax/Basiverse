using System;
using Basiverse;
using System.Collections.Generic;

namespace Basiverse
{
    [Serializable]
    class Hull{ // Hulls have a name, a current value, a max value and a heat max
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}

        private double _hullval;
        public double Hullval{ get {return _hullval;} set {_hullval = value;}}
        private double _maxhull;
        public double HullMax{ get {return _maxhull;} set {_maxhull = value;}}

        private double _maxheat;
        public double HeatMax{ get {return _maxheat;} set {_maxheat = value;}}

        public double Health(){
            double tmp = (_hullval/_maxhull);
            tmp = tmp * 100;
            return tmp;
        }

        public Hull(string inName, double inMax, double inHeatMax){
            _name = inName;
            _maxhull = inMax;
            _hullval = inMax;
            _maxheat = inHeatMax;
        }

        public Hull(){

        }

        public string GetHullColor(){
            double hullSw = Math.Floor(Health());
            if(hullSw >= 75){
                return "green";
            }
            else if (hullSw <= 25){
                return "red";
            }
            else{
                return "yellow";
            }
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

        public Armor(string inName, int inVal, int inCost){
            _name = inName;
            _armorVal = inVal;
            _cost = inCost;
        }

        public Armor(){

        }
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


        public Heatsink(string inName, int inPassive, int inActive, int inCost){
            _name = inName;
            _passiveval = inPassive;
            _activeval = inActive;
            _cost = inCost;
            _isactive = false;
        }

        public Heatsink(){

        }

        public string GetOnlineStr(){
            if (_isactive){

                return "[green]Active Cooling[/]";
            }
            else{
                return "[red]Passive Cooling[/]";
            }
        }

        public string GetHeatColor(double HeatVal){
            double heatSw = Math.Floor(HeatVal);
            if(heatSw <= 25){
                return "green";
            }
            else if(heatSw <= 75 && heatSw > 25){
                return "yellow";
            }
            else{
                return "red";
            }
        }
    }
    
    [Serializable]
    class Shield{ // Shield have a name, a value, a max value, an online flag and a cost
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}

        private double _shieldval;
        public double ShieldVal{ get {return _shieldval;} set {_shieldval = value;}}
        private double _maxshield;
        public double ShieldMax{ get {return _maxshield;} set {_maxshield = value;}}
        private bool _isonline = true;
        public bool IsOnline{get {return _isonline;} set {_isonline = value;}}
        public double Health(){
            return (_shieldval/_maxshield) * 100;
        }

        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}


        public Shield(string inName, double inMax, int inCost){
            _name = inName;
            _maxshield = inMax;
            _shieldval = inMax;
            _isonline = true;
            _cost = inCost;
        }

        public Shield(){ // Default constructor

        }

        public void Regen(){ // if the shields are online regen 5%
            if(_isonline && (_shieldval < _maxshield)){
                _shieldval = _shieldval * 1.2;
            }
            else{
                return;
            }
        }

        public string GetShieldColor(){ 
            double shieldSw = Math.Floor(Health());
            if( shieldSw >= 75){
                return "green";
            }
            else if(shieldSw < 75 && shieldSw > 25 ){
                return "yellow";
            }
            else if(shieldSw == 0){
                return "red";
            }
            else{
                return "darkorange";
            }
        }
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

        public Laser(string inName, int inDamage, int inHeat, int inCost){
            _name = inName;
            _damage = inDamage;
            _heat = inHeat;
            _cost = inCost;
        }

        public Laser(){

        }
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

        public Missile(string inName, int inDamage, double inHitchance, int inCost){
            _name = inName;
            _damage = inDamage;
            _hitchance = inHitchance;
            _cost = inCost;
        }

        public Missile(){

        }

    }

    [Serializable]
    class Engine{ // Engines have a name a flee chance and a cost
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private double _fleechance;
        public double FleeChance{get {return _fleechance;} set {_fleechance = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}

        public Engine(string inName, double inFlee, int inCost){
            _name = inName;
            _fleechance = inFlee;
            _cost = inCost;
        }

        public Engine(){

        }
        
    }

    [Serializable]
    class CargoHold{ // Cargo Holds have a name, a max size, a current size and a list of cargo
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private int _maxSize = 0;
        public int MaxSize{ get {return _maxSize;} set {_maxSize = value;}}

        private int _currSize = 0;
        public int CurrentSize{ get {return _currSize;} set {_currSize = value;}}

        public CargoHold(string inName, int inMax){
            _name = inName;
            _maxSize = inMax;
        }

        public CargoHold(){

        }

        public int Available(){
            return _maxSize - _currSize;
        }
    }
    
    [Serializable]
    class Cargo{ // Cargo has a name, a size in m3, a type, a count and a cost
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private int _size = 0;
        public int Size{ get {return _size;} set {_size = value;}}

        private double _cost = 0;
        public double Cost{ get {return _cost;} set {_cost = value;}}

        private int _type = 0;
        public int Type{ get {return _type;} set {_type = value;}}

        private string _description = "";
        public string Description{ get {return _description; } set {_description = value;}}

        private double _adjustedPrice = 0;
        public double AdjustedPrice{ get {return _adjustedPrice; } set {_adjustedPrice = value;}}

        private int _count = 1;
        public int Count{ get {return _count; } set {_count = value;}}
    
        public Cargo(string inName, int inSize, double inCost, int inType, string inDescription){
            _name = inName;
            _size = inSize;
            _cost = inCost;
            _type = inType;
            _description = inDescription;
        }

        public Cargo(){

        }
    
    }

    [Serializable]
    class Chassis{ // Chassis have a Name, a Hull, A Cargohold, a Cost and a Description;
        private string _name = "";
        public string Name{get {return _name;} set {_name = value;}}
        private string _description = "";
        public string Description{get {return _description;} set {_description = value;}}
        private int _cost = 0;
        public int Cost{get {return _cost;} set {_cost = value;}}
        public Hull _Hull;
        public CargoHold _Cargohold;

        public Chassis(string inName, string inDescription, int inCost, Hull inHull, CargoHold inHold){
            _name = inName;
            _description = inDescription;
            _cost = inCost;
            _Hull = inHull;
            _Cargohold = inHold;
        }

        public Chassis(){

        }
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}