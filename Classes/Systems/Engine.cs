using System;

namespace Basiverse{
    
    [Serializable]
    class Engine:BaseSystem{ // Engines have a name a flee chance and a cost
        private double _fleechance;
        public double FleeChance{get {return _fleechance;} set {_fleechance = value;}}

        public Engine(string inName, double inFlee, int inCost){
            Name = inName;
            Cost = inCost;
            _fleechance = inFlee;
        }
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}