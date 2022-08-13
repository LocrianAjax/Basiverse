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

        // Debugging stuff
        public void Dump(){ // Checks to make sure that the data passes in correctly.
            AnsiConsole.MarkupLine("Dumping Data about player for this game instance");
            AnsiConsole.MarkupLine("Player Name: {0}", mainPlayer.Name);
            AnsiConsole.MarkupLine("Player Money: {0}", mainPlayer.Money);
            AnsiConsole.MarkupLine("Player Ship data\n");
            mainPlayer.PShip.DisplayData();
            mainPlayer.PShip.ShowStats();
        }

        public Player TestSave(){
            Loader MainLoad = new Loader(); // Create the Loader
            Saver MainSave = new Saver(); // Create the Loader

            MainSave.SaveData(0, "Test", mainPlayer);
            AnsiConsole.MarkupLine("Saved {0} to file", mainPlayer.Name);

            return MainLoad.LoadSave();
        }
        // End Debugging stuff

        public void Start(){ // Starts a new game, every other method from here on out is private
            Console.Clear(); // Clear the console and write the UI
            WriteStatus();
            var tmp = AnsiConsole.Prompt(
                new TextPrompt<string>("Press any key to continue")
                .AllowEmpty());
        }

        private void WriteStatus(){ // Create our Default "NAV" Screen
            /*
                NAV             | STATUS
                Location info      Shield
                POIS                Hull
                                    Etc
            */

            // Add data for nav table
            Table NavScreen = new Table();
            NavScreen.AddColumn(mainPlayer.PLoc.Name); 
            NavScreen.AddColumn(mainPlayer.PLoc.Description);
            if(mainPlayer.PLoc.Interests != null){
                foreach(PointofInterest poi in mainPlayer.PLoc.Interests){
                NavScreen.AddRow($"{poi.Name}: {poi.Description}");
                }
            }

            // Add data for status table
            Table StatusScreen = new Table();
            StatusScreen.AddColumn(mainPlayer.PShip.Name);
            StatusScreen.AddRow(mainPlayer.PShip.StatusStr());

            // Setup main table
            Table MainScreen = new Table();
            MainScreen.AddColumn("NAV");
            MainScreen.AddColumn("STATUS");
            MainScreen.AddRow(NavScreen, StatusScreen);
            MainScreen.Expand();
            AnsiConsole.Write(MainScreen);
        }
        
        // Combat loop: 2 actions then check heat, and check for death.
    }
}