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

        public int Decide(){ // Decides actions for combat NPCs
            // Return 0 for flee, 1 for activate heatsink, 2 for laser, 3 for missiles, 4 to restart shields
            var rand = new Random();

            if(cShip.HullVal() <= 75){ // If hull is < 75% 20% chance to flee, increase chance by 5% for each 5% hull decrease
                int fleeRand = rand.Next(0, 101);
                double remval = (75 - cShip.HullVal());
                if(fleeRand <= (20 + remval)){
                    return 0; // Check for flee if not, fall down to the attack section
                }
            }

            if(cShip.HeatVal() > 75){ // If heat is > 75% 50% chance to shoot or activate heatsink
                if(cShip.HeatVal() > 95){ // If heat is > 95% activate heatsink
                    return 1; // If heat is too high, always activate the sink
                }
                else{
                    int heatRand = rand.Next(0, 101);
                    if(heatRand > 50){
                        return 1;
                    } // Otherwise we keep falling down to fire
                }
            }

            if(!cShip.Shield.IsOnline){ // If the shield is offline, 75% chance to restart
                int shieldRand = rand.Next(0, 101);
                if(shieldRand <= 75){
                    return 4;
                }
            }


            if(cShip.Missile.Stock > 0){// If missiles > 0 shoot missiles or laser 50% chance
                int fireRand = rand.Next(0, 101);
                if(fireRand > 50){
                    return 3;
                }
                else{
                    return 2;
                }
            }
            else{ // If we're out of missiles and we've fallen this far, fire lasers
                return 2;
            }
        }
    }
}