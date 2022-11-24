using System;

namespace Basiverse{

    [Serializable]
    class Chassis:BaseSystem{ // Chassis have a Name, a Hull, A Cargohold, a Cost and a Description;
        public Hull _Hull;
        public CargoHold _Cargohold;

        public Chassis(string inName, string inDescription, int inCost, Hull inHull, CargoHold inHold){
            Name = inName;
            Description = inDescription;
            Cost = inCost;
            _Hull = inHull;
            _Cargohold = inHold;
        }

        public Chassis(){

        }
    }
}