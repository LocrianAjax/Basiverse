using System;

namespace Basiverse{
    
    [Serializable]
    class Heatsink{ // Heatsinks have a name, a pasive value, an active value, an active flag, and a cost
        private string _name = "None";
        public string Name{ get {return _name;} set {_name = value;}}

        private int _passiveval = 0;
        public int PassiveVal{ get {return _passiveval;} set {_passiveval = value;}}

        private int _activeval = 0;
        public int ActiveVal{ get {return _activeval;} set {_activeval = value;}}

        private bool _isactive = false;
        public bool IsActive{ get {return _isactive;} set {_isactive = value;}}
        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}


        public Heatsink(string inName, int inPassive, int inActive, int inCost){
            _name = inName;
            _passiveval = inPassive;
            _activeval = inActive;
            _cost = inCost;
            _isactive = false;
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