using System;
using Basiverse;
using Spectre.Console;

namespace Basiverse{

    class Game{

        // A game has a player
        Player mainPlayer;

        public Game(Player starter){
            mainPlayer = starter;
        }

        public void Dump(){ // Checks to make sure that the data passes in correctly.
            AnsiConsole.MarkupLine("Dumping Data about player for this game instance");
            AnsiConsole.MarkupLine("Player Name: {0}", mainPlayer.Name);
            AnsiConsole.MarkupLine("Player Money: {0}", mainPlayer.Money);
            AnsiConsole.MarkupLine("Player Ship data\n");
            mainPlayer.PShip.DisplayData();
            mainPlayer.PShip.ShowStats();
        }

        public void Start(){ // Starts a new game, every other method from here on out is private
            Console.Clear(); // Clear the console and write the UI
        }

        public Player TestSave(){
            Loader MainLoad = new Loader(); // Create the Loader
            Saver MainSave = new Saver(); // Create the Loader

            MainSave.SaveData(0, "Test", mainPlayer);
            AnsiConsole.MarkupLine("Saved {0} to file", mainPlayer.Name);

            return MainLoad.LoadSave();
        }
        
        // Combat loop: 2 actions then check heat, and check for death.
    }
}