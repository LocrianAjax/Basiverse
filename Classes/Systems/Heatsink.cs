using System;

namespace Basiverse{
    
    [Serializable]
    class Heatsink:BaseSystem{ // Heatsinks have a name, a pasive value, an active value, an active flag, and a cost
        private int _passiveval = 0;
        public int PassiveVal{ get {return _passiveval;} set {_passiveval = value;}}

        private int _activeval = 0;
        public int ActiveVal{ get {return _activeval;} set {_activeval = value;}}

        private bool _isactive = false;
        public bool IsActive{ get {return _isactive;} set {_isactive = value;}}

        private int _corecount = 0;
        public int CoreCount{ get {return _corecount;} set {_corecount = value;}}

        public Heatsink(string inName, int inPassive, int inActive, int inCost){
            Name = inName;
            Cost = inCost;
            _passiveval = inPassive;
            _activeval = inActive;
            _isactive = false;
            _corecount = 3;
        }

        public Heatsink(){

        }

        public string GetOnlineStr(){
            if (_isactive){

                return "[green]Active Cooling[/]";
            }
            else{
                return "[red]Passive Cooling[/]";
            }
        }

        public string GetHeatColor(double HeatVal){
            double heatSw = Math.Floor(HeatVal);
            if(heatSw <= 25){
                return "green";
            }
            else if(heatSw <= 75 && heatSw > 25){
                return "yellow";
            }
            else{
                return "red";
            }
        }
    }

    /* Future Ideas: 
        Cloaking Device
        Weapons Jammer
    */


}