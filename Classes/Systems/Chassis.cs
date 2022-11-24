using System;

namespace Basiverse{

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