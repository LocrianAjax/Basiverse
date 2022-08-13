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

        public Ship PShip;

        public Map PMap;

        public Location PLoc;
    }
}
