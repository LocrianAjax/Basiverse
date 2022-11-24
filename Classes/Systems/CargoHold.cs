using System;

namespace Basiverse{
    
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

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}