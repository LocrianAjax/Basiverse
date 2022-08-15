using System;
using Basiverse;
using Spectre.Console;
using System.Threading;

namespace Basiverse{

    [Serializable]
    class Station{
        // A station has a name, a type, a list of NPCs
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private string _type = "";
        public string Type{ get {return _type;} set {_type = value;}}
        private string _description = "";
        public string Description{ get {return _description;} set {_description = value;}}

        public Station(){ // Default constructor
            _name = "Derelict";
            _type = "Wreck";
            _description = "A former station now abandoned.";
        }

        public Station(string inName, string inType, string inDescription){ // Overloaded constructor
            _name = inName;
            _type = inType;
            _description = inDescription;
        }
    }
}