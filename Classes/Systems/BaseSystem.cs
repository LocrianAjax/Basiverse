using System;

namespace Basiverse{

    [Serializable] 
    class BaseSystem{ // Contains all the shared components of the systems
        private string _name = "";
        public string Name{ get {return _name; } set {_name = value;}}

        private int _cost = 0;
        public int Cost{ get {return _cost;} set {_cost = value;}}

        private string _description = "";
        public string Description{get {return _description;} set {_description = value;}}
    }
}