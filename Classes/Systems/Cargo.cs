using System;

namespace Basiverse{
    
    [Serializable]
    class Cargo:BaseSystem{ // Cargo has a name, a size in m3, a type, a count and a cost

        private int _size = 0;
        public int Size{ get {return _size;} set {_size = value;}}


        private int _type = 0;
        public int Type{ get {return _type;} set {_type = value;}}

        private double _adjustedPrice = 0;
        public double AdjustedPrice{ get {return _adjustedPrice; } set {_adjustedPrice = value;}}

        private int _count = 1;
        public int Count{ get {return _count; } set {_count = value;}}
    
        public Cargo(string inName, int inSize, int inCost, int inType, string inDescription){
            Name = inName;
            _size = inSize;
            Cost = inCost;
            _type = inType;
            Description = inDescription;
        }

        public Cargo(){

        }
    
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}