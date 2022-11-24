using System;

namespace Basiverse{
    
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

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}