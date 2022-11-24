using System;

namespace Basiverse{
    
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

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}