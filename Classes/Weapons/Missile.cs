using System;

namespace Basiverse{
    
    [Serializable]    
    class Missile:Weapon{ // Missiles have a name, a hit chance, damage value, a stock value and a cost

        private double _hitchance;
        public double HitChance{get {return _hitchance;} set {_hitchance = value;}}
        private int _stock;
        public int Stock{get {return _stock;} set {_stock = value;}}

        public Missile(string inName, int inDamage, double inHitchance, int inCost){
            Name = inName;
            Damage = inDamage;
            _hitchance = inHitchance;
            Cost = inCost;
        }

        public Missile(){

        }
    }
}