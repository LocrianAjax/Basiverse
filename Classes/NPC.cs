using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;

namespace Basiverse
{

    [Serializable]
    class Dialoug{ // Dialouges contain a set, line and a bool for if it's an intro line
        public int set;
        public string line;

        public bool isIntro;
    }

    [Serializable]
    class NPC{ // An NPC contains has a name, a type, and a list of dialouge
        private string _name;
        public string Name{ get {return _name;} set {_name = value;}}

        private int _type = 0;
        public int Type{ get {return _type;} set {_type = value;}}

        // NPC Types will be: 0 - Basic, 1 - Combat, 2 - Trade

        public List<Dialoug> Dialouges;

        public Ship cShip; // For combat NPCs
        public int difficulty;
    
        public NPC(string inName, int inDifficulty){ // Overloaded constructor for making combat NPCs
            _name = inName;
            _type = 1;
            difficulty = inDifficulty;
        }
    
    }
}