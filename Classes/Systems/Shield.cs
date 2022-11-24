using System;

namespace Basiverse{

    [Serializable]
    class Shield:BaseSystem{ // Shield have a name, a value, a max value, an online flag and a cost
        private double _shieldval;
        public double ShieldVal{ get {return _shieldval;} set {_shieldval = value;}}
        private double _maxshield;
        public double ShieldMax{ get {return _maxshield;} set {_maxshield = value;}}
        private bool _isonline = true;
        public bool IsOnline{get {return _isonline;} set {_isonline = value;}}
        public double Health(){
            return (_shieldval/_maxshield) * 100;
        }

        public Shield(string inName, double inMax, int inCost){
            Name = inName;
            _maxshield = inMax;
            _shieldval = inMax;
            _isonline = true;
            Cost = inCost;
        }

        public Shield(){ // Default constructor

        }

        public void Regen(){ // if the shields are online regen 10%
            if(_isonline && (_shieldval < _maxshield)){
                _shieldval = _shieldval * 1.1;
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

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}