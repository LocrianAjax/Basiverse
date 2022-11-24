using System;

namespace Basiverse{
    
    [Serializable]
    class Hull:BaseSystem{ // Hulls have a name, a current value, a max value and a heat max

        private double _hullval;
        public double Hullval{ get {return _hullval;} set {_hullval = value;}}
        private double _maxhull;
        public double HullMax{ get {return _maxhull;} set {_maxhull = value;}}

        private double _maxheat;
        public double HeatMax{ get {return _maxheat;} set {_maxheat = value;}}

        public double Health(){
            double tmp = (_hullval/_maxhull);
            tmp = tmp * 100;
            return tmp;
        }

        public Hull(string inName, double inMax, double inHeatMax){
            Name = inName;
            _maxhull = inMax;
            _hullval = inMax;
            _maxheat = inHeatMax;
        }

        public Hull(){

        }

        public string GetHullColor(){
            double hullSw = Math.Floor(Health());
            if(hullSw >= 75){
                return "green";
            }
            else if (hullSw <= 25){
                return "red";
            }
            else{
                return "yellow";
            }
        }

    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}