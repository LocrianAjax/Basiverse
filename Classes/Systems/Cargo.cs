using System;

namespace Basiverse{
    
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

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}