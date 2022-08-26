using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;

namespace Basiverse
{

    [Serializable]
    class Combat{ // A combat contains an NPC and then classes that take in a player
        public NPC NPC;
    
        Combat(int difficulty){ // Generates a new combat with the specified difficulty
            var rand = new Random();
            Generator CombatGen = new Generator();
            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data\\combatnpcnames.data");
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0) + 1))], difficulty);
        }

        public void Fight(Player inPlayer){
            // Simmilar setup to the station start/loop
            while(true){
                // Do stuff
                AnsiConsole.Clear();
                CombatScreen(inPlayer);
                CombatMenu();
                inPlayer.PShip.CheckHeat();
                NPC.cShip.CheckHeat();
                if(inPlayer.PShip.CheckDestroyed()){
                    // GameOver();
                }
                else if(NPC.cShip.CheckDestroyed()){
                    // Victory();
                }
            }
        }

        public void CombatScreen(Player inPlayer){
            // TODO: This
        }

        public void CombatMenu(){
            // TODO: This
        }

    }

}