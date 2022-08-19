using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;

namespace Basiverse
{
    [Serializable]
    class NPC{ // An NPC contains has a name, a type, and a list of dialouge
        private string _name;
        public string Name{ get {return _name;} set {_name = value;}}

        private int _type = 0;
        public int Type{ get {return _type;} set {_type = value;}}

        // NPC Types will be: 0 - Basic, 1 - Combat, 2 - Trade

        public List<Dialoug> Dialougs;

        public Ship cShip; // For combat NPCs
    }

    [Serializable]
    class Dialoug{ // Dialouges contain a line, and ???
        public string line;

        public bool isHidden;

        public bool isLocked;

        public int unLocks;

    }
}