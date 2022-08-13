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
        }

        public Player TestSave(){
            Loader MainLoad = new Loader(); // Create the Loader
            Saver MainSave = new Saver(); // Create the Loader

            MainSave.SaveData(0, "Test", mainPlayer);
            AnsiConsole.MarkupLine("Saved {0} to file", mainPlayer.Name);

            return MainLoad.LoadSave();
        }
        // End Debugging stuff

        public void Start(){ // Entry point for the game
            Console.Clear(); // Clear the console and write the UI
            WriteStatus();
            MainActionMenu();
        }

        private void WriteStatus(){ // Create our Default "NAV" Screen
            /*
                NAV          | STATUS  | NEARBY
                Location info  Shield    Name Desc
                POIS           Hull
                               Etc
            */

            // Add data for nav table
            Table NavScreen = new Table();
            NavScreen.AddColumn(mainPlayer.PLoc.Name); 
            NavScreen.AddColumn(mainPlayer.PLoc.Description);
            if(mainPlayer.PLoc.Interests != null){
                foreach(PointofInterest poi in mainPlayer.PLoc.Interests){
                NavScreen.AddRow($"{poi.Name}: {poi.Description}");
                NavScreen.AddEmptyRow();
                }
            }

            // Add data for status table
            Table StatusScreen = new Table();
            StatusScreen.AddColumn(mainPlayer.PShip.Name);
            // Add rows for Hull/Heat/Shield

            // Shield Status
            if(mainPlayer.PShip.ShieldVal() >= 75){
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [green]{mainPlayer.PShip.ShieldVal()}[/]%"));
            }
            else if(mainPlayer.PShip.ShieldVal() < 75 && mainPlayer.PShip.ShieldVal() > 25 ){
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [yellow]{mainPlayer.PShip.ShieldVal()}[/]%"));
            }
            else if(mainPlayer.PShip.ShieldVal() == 0){
                StatusScreen.AddRow(new Markup($"Shields [red][rapidblink]OFFLINE[/][/]"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [orange]{mainPlayer.PShip.ShieldVal()}[/]%"));
            }

            // Hull Status
            if(mainPlayer.PShip.HullVal() >= 75){
                StatusScreen.AddRow(new Markup($"Hull Integrity: [green]{mainPlayer.PShip.HullVal()}[/]%"));
            }
            else if (mainPlayer.PShip.HullVal() <= 25){
                StatusScreen.AddRow(new Markup($"Hull Integrity: [red]{mainPlayer.PShip.HullVal()}[/]%"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Hull Integrity: [yellow]{mainPlayer.PShip.HullVal()}[/]%"));
            }

            // Heat Status
            if(mainPlayer.PShip.HeatVal() <= 25){
                StatusScreen.AddRow(new Markup($"Heat Soak - [green]{mainPlayer.PShip.HeatVal()}[/]%"));
            }
            else if(mainPlayer.PShip.HeatVal() <= 75 && mainPlayer.PShip.HeatVal() > 25){
                StatusScreen.AddRow(new Markup($"Heat Soak - [yellow]{mainPlayer.PShip.HeatVal()}[/]%"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Heat Soak - [red]{mainPlayer.PShip.HeatVal()}[/]%"));
            }

            // Add data for nearby table
            Table NearbyScreen = new Table();
            NearbyScreen.AddColumns("Name","Desc");
            foreach(Location nearby in mainPlayer.PLoc.NearbyNodes){
                NearbyScreen.AddRow(nearby.Name,nearby.Description);
                NearbyScreen.AddEmptyRow();
            }

            // Setup main table
            Table MainScreen = new Table();
            MainScreen.AddColumns("NAVIGATION REPORT", "STATUS REPORT", "NEARBY SYSTEMS");
            MainScreen.AddRow(NavScreen, StatusScreen, NearbyScreen);
            MainScreen.Expand();
            MainScreen.Border(TableBorder.Ascii);
            AnsiConsole.Write(MainScreen);
        }
        
        private void MainActionMenu(){ // Menu for Main Game actions
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Actions:")
                .PageSize(3)
                .AddChoices(new[] { "Jump", "Detailed Report",  "Options"}));
            
            switch(selection){
                case "Jump":
                    JumpMenu();
                break;
                case "Detailed Report":
                    DetailedReportMenu();
                break;
                case "Options":
                    OptionsMenu();
                break;
            }
        }

        private void JumpMenu(){

        }
        private void OptionsMenu(){
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Options:")
                .PageSize(4)
                .AddChoices(new[] { "Settings", "Return", "Save", "Quit"}));
            
            switch(selection){
                case "Settings":
                    SettingsMenu();
                break;
                case "Return":
                    MainActionMenu();
                break;
                case "Save":
                    SaveGame();
                break;
                case "Quit":
                    return;
            }
        }

        private void DetailedReportMenu(){

        }

        private void SettingsMenu(){

        }

        private void SaveGame(){

        }
        // Combat loop: 2 actions then check heat, and check for death.
    }
}