using System;
using Basiverse;
using Spectre.Console;

namespace Basiverse
{
    [Serializable]
    class Player
    {
        // A player has a name, money, ship, and location
        private string _name;
        public string Name{ get {return _name;} set {_name = value;}}

        private int _money = 0; // Me too thanks
        public int Money{get {return _money;} set {_money = value;}}

        private int _jumpLog = 0; // Logs num jumps
        public int JumpLog{get {return _jumpLog;} set {_jumpLog = value;}}

        public Ship PShip;

        public Map PMap;

        public Location PLoc;
        
        public String CapitansLog; // TODO

        public bool Rename(string newName){
            if(String.IsNullOrEmpty(newName) || String.IsNullOrWhiteSpace(newName)){
                return false; // Return false if there's no name
            }
            else{
                _name = newName;
                return true;
            }
        }
    }
}
