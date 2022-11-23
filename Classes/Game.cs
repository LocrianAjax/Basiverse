using System;
using Basiverse;
using Spectre.Console;
using System.Threading;

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
            Saver MainSave = new Saver(); // Create the Saver

            MainSave.SaveData(mainPlayer);
            AnsiConsole.MarkupLine("Saved {0} to file", mainPlayer.Name);

            return MainLoad.LoadSave();
        }
        // End Debugging stuff

        public void Start(){ // Entry point for the game
            int retval = 0;
            while(retval == 0){
                Console.Clear(); // Clear the console and write the UI
                WriteStatus();
                retval = MainActionMenu();
            }  
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
            NavScreen.AddColumns("Local System", ""); 
            NavScreen.AddRow(mainPlayer.PLoc.Name, mainPlayer.PLoc.Description);

            Table PoiScreen = new Table();
            if(mainPlayer.PLoc.Interests != null){
                PoiScreen.AddColumns("System Scan Data", "");
                foreach(PointofInterest poi in mainPlayer.PLoc.Interests){
                PoiScreen.AddRow($"{poi.Name}", $"{poi.Description}");
                PoiScreen.AddEmptyRow();
                }
            }
            else{
                PoiScreen.AddColumn("System Scan Data");
                PoiScreen.AddRow($"Nothing interesting here");
            }

            // Add data for status table
            Table StatusScreen = new Table();
            StatusScreen.AddColumn(mainPlayer.PShip.Name);

            // Add rows for Hull/Heat/Shield
            // Shield Status
             if(mainPlayer.PShip.Shield.IsOnline){
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [{mainPlayer.PShip.Shield.GetShieldColor()}]{mainPlayer.PShip.ShieldVal()}[/]%"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }

            // Hull Status 
            StatusScreen.AddRow(new Markup($"Hull Integrity: [{mainPlayer.PShip.Hull.GetHullColor()}]{mainPlayer.PShip.HullVal()}[/]%"));

            // Heat Status
            StatusScreen.AddRow(new Markup($"{mainPlayer.PShip.Heatsink.GetOnlineStr()} Heat Soak - [{mainPlayer.PShip.Heatsink.GetHeatColor(mainPlayer.PShip.HeatVal())}]{mainPlayer.PShip.HeatVal()}[/]%"));

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
            MainScreen.AddRow(PoiScreen);
            MainScreen.Expand();
            MainScreen.Border(TableBorder.Ascii);
            AnsiConsole.Write(MainScreen);
        }
        
        private int MainActionMenu(){ // Menu for Main Game actions

             // Set up choices
            string choices = "Jump|";
            // Check and see if we have any stations
            if(mainPlayer.PLoc.Interests != null){
                foreach(PointofInterest temp in mainPlayer.PLoc.Interests){
                    if(temp.Type == 2){
                        choices += "Dock|";
                        break;
                    }
                    if(temp.Name == "Mineable Asteroids"){
                        choices += "Mine|";
                    }
                }
            }

            choices += "Detailed Report|Options|Manual";
            // Then split on | to create dynamic menu
            string[] options = choices.Split('|');
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Actions:")
                .PageSize(options.Length + 3)
                .AddChoices(options));
            
            switch(selection){
                case "Jump":
                    return JumpMenu();
                case "Detailed Report":
                    return DetailedReportMenu();
                case "Options":
                    return OptionsMenu();
                case "Dock":
                    return DockMenu();
                case "Mine":
                    return MiningMenu();
                case "Manual":
                    return ManualMenu();
            }
            return 0;
        }

        private int ManualMenu(){
            Manual MainMenu = new Manual();
            MainMenu.Load("Data//Manuals//main.data");
            MainMenu.Display();
            return 0;
        }
        private int MiningMenu(){
            var rand = new Random();
            MiningMinigame NewGame = new MiningMinigame(rand.Next(3, 11)); // Generate a random sized asteroid
            NewGame.StartMinigame(mainPlayer);
            return 0;
        }

        private int JumpMenu(){
            string[] Locations = new string[mainPlayer.PLoc.NearbyNodes.Count + 1]; // Set up the array of options to jump
            Locations[mainPlayer.PLoc.NearbyNodes.Count] = "Return"; // Add to end of options list
            for(int i = 0; i < mainPlayer.PLoc.NearbyNodes.Count; i++){
                Locations[i] = mainPlayer.PLoc.NearbyNodes[i].Name;
            }
            int pageCount = mainPlayer.PLoc.NearbyNodes.Count + 1;
            if(pageCount <= 3){pageCount = 4;}

            string destination = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a Destination:")
                .PageSize(pageCount)
                .AddChoices(Locations));

            if(destination == "Return"){
                return 0;
            }
            else{
                foreach(Location temp in mainPlayer.PLoc.NearbyNodes){
                    if(destination == temp.Name){
                        mainPlayer.logJump(destination); // Log the jump when confirmed
                        // Regen shield and cool the ship
                        mainPlayer.PShip.TravelPassive();
                        mainPlayer.PLoc = temp;
                        mainPlayer.JumpLog += 1;
                        // This is just for show
                        var randProg = new Random();
                        AnsiConsole.Progress().HideCompleted(false).Start(ctx => {
                            var warmEngine = ctx.AddTask("[red]Warming warp engine up to operating temperature[/]");
                            var jumping = ctx.AddTask("[green]Jumping[/]");
                            while(!warmEngine.IsFinished){
                                warmEngine.Increment(randProg.Next(1,10));
                                Thread.Sleep(20);
                            }
                            while(!jumping.IsFinished){
                                jumping.Increment(randProg.Next(5,25));
                                Thread.Sleep(100);
                            }
                        });
                        
                        // After we jump, check for a random combat encounter
                        // 10% chance for a random encounter, difficulty scaling with jump numbers
                        var rand = new Random();
                        int combatChance = rand.Next(0, 101);
                        if(combatChance <= 10){
                            AnsiConsole.Clear();
                            AnsiConsole.WriteLine("As you exit your jump and prepare to scan the system the shrill sound of an enemy target lock disrupts your thoughts");
                            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Prepare to fight!").AllowEmpty());
                            int retval = Combat(mainPlayer.GetDifficulty());
                            if(retval == -1){
                                return 0;
                            }
                        }
                        // If we don't do combat, just go back to the normal loop
                        return 0; // After that, return to the loop
                    }
                }
            }
            return 0;
        }

        private int OptionsMenu(){
            string opts = "Settings|Return|Save|Save and Quit|Quit without Saving|Return to Main Menu";
            if(Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["debugMode"])){
                opts += "|Debug";
            }
            string[] options = opts.Split('|');
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Options:")
                .PageSize(6)
                .AddChoices(options)); 
            
            switch(selection){
                case "Settings":
                    SettingsMenu();
                break;
                case "Return":
                    return 0;
                case "Save":
                    SaveGame();
                    return 0;
                case "Save and Quit":
                    SaveGame();
                    System.Environment.Exit(0);
                break;
                case "Debug":
                    DebugMenu();
                break;
                case "Return to Main Menu":
                    return 1;
                case "Quit without Saving":
                   System.Environment.Exit(0);
                break;
            }
            return 0;
        }

        private int DetailedReportMenu(){
            AnsiConsole.Clear();
            // Add data for Detailed Ship Report
            /*
                SYSTEM  |   REPORT
                Shield  |   info
                etc.    |   etc.
            */
            Table ReportScreen = new Table();
            ReportScreen.Title = new TableTitle($"The {mainPlayer.PShip.Name} DETAILED SYSTEMS REPORT. CHASSIS: {mainPlayer.PShip.Chassis} CAPITAN: {mainPlayer.Name}");
            ReportScreen.AddColumns("SYSTEM","REPORT");
            /*
                Shield 
                Hull
                Armor
                Heatsink
                Engine
                Lasers
                Missiles
                Cargohold
            */
            ReportScreen.AddRow($"Bank Account",$"{mainPlayer.Money}");
            // Shield Status
            if(mainPlayer.PShip.Shield.IsOnline){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"Shields [cyan]ONLINE[/] - Strength: [{mainPlayer.PShip.Shield.GetShieldColor}]{mainPlayer.PShip.ShieldVal()}[/]%"));
            }
            else{
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }

            // Hull Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Hull.Name}"), new Markup($"Hull Integrity: [{mainPlayer.PShip.Hull.GetHullColor()}]{mainPlayer.PShip.HullVal()}[/]%"));
            
            // Armor Data
            if(mainPlayer.PShip.Armor.Name != "None"){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Armor.Name}"), new Markup($"Damage Mitigation: {mainPlayer.PShip.Armor.ArmorValue}"));
            }
            else{
                ReportScreen.AddRow(new Markup($"No Armor installed"));
            }

            // Heat Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Heatsink.Name}"), new Markup($"{mainPlayer.PShip.Heatsink.GetOnlineStr()} Heat Soak - [{mainPlayer.PShip.Heatsink.GetHeatColor(mainPlayer.PShip.HeatVal())}]{mainPlayer.PShip.HeatVal()}[/]%"));


            // Engine Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Engine.Name}"), new Markup($"Flee Chance: {mainPlayer.PShip.Engine.FleeChance * 100}%"));
            // Laser Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Laser.Name}"), new Markup($"Damage: {mainPlayer.PShip.Laser.Damage} Heat Generated: {mainPlayer.PShip.Laser.Heat}"));
            // Missile Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Missile.Name}"), new Markup($"Damage: {mainPlayer.PShip.Missile.Damage} Hit Chance: {mainPlayer.PShip.Missile.HitChance}% Stock: {mainPlayer.PShip.Missile.Stock}"));
            // Cargohold Status
            ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Hold.Name}"), new Markup($"In Use: {mainPlayer.PShip.Hold.CurrentSize}m^3 Free: {mainPlayer.PShip.Hold.MaxSize}m^3"));
            if(mainPlayer.PShip.CargoHold != null){
                foreach(Cargo item in mainPlayer.PShip.CargoHold){
                    ReportScreen.AddRow(new Markup(""), new Markup($"Name: {item.Name} Size: {item.Size}m^3 Cost: {item.Cost}"));
                }
            }
            ReportScreen.Expand();
            AnsiConsole.Write(ReportScreen);
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to return").AllowEmpty());
            return 0;
        }

        private void SettingsMenu(){
            // Player / Ship Rename
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Settings:")
                .PageSize(4)
                .AddChoices(new[] { "Rename Player", "Rename Ship", "Return",}));
            
            switch(selection){
                case "Rename Player":
                    RenamePlayer();
                break;
                case "Rename Ship":
                    RenameShip();
                break;
                case "Return":
                    OptionsMenu();
                break;
            }
        }

        private void RenamePlayer(){
            bool res = false;
            while(res != true){
                string Name = AnsiConsole.Ask<string>("Please enter your new name:");
                res = mainPlayer.Rename(Name);
            }
            Console.Clear(); // Clear the console and write the UI
            WriteStatus();
            return;
        }

        private void RenameShip(){
            bool res = false;
            while(res != true){
                string Name = AnsiConsole.Ask<string>("Please enter your ship's new name:");
                res = mainPlayer.PShip.Rename(Name);
            }
            Console.Clear(); // Clear the console and write the UI
            WriteStatus();
            return;
        }

        private int SaveGame(){
            Saver MainSave = new Saver(); // Create the Saver
            AnsiConsole.MarkupLine("Saving player data");
            try{
                MainSave.SaveData(mainPlayer); // Save player as bin
            }
            catch (Exception e){
                AnsiConsole.WriteException(e);
            }
            AnsiConsole.MarkupLine("Save [green]Complete[/]");
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
            return 0;
        }

        private int DockMenu(){
            string Stations = ""; // Set up the array of options to dock
            foreach(PointofInterest poi in mainPlayer.PLoc.Interests){
                if(poi.Type == 2){ // If it's a station add it to the list
                    Stations += $"{poi.Name}|";
                }
            }
            Stations += "Return";
            string[] options = Stations.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}
            string destination = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a Destination:")
                .PageSize(pageCount)
                .AddChoices(options));

            if(destination == "Return"){
                return 0;
            }
            else{
                foreach(Station tmp in mainPlayer.PLoc.Stations){ // Not effcient, but we're going to a max of 5 things so no biggie
                    if(tmp.Name == destination){
                        DockingMinigame Docker = new DockingMinigame();
                        bool res = Docker.StartMinigame();
                        if(res){
                            mainPlayer.logDock(tmp.Name); // Log Docking 
                            tmp.Dock(mainPlayer);
                        }
                        return 0;
                    }
                }
            }
            return 0;
        }

        private int Combat(int difficulty){
            Combat myCombat = new Combat(difficulty);
            int retval = myCombat.Fight(mainPlayer);
            if(retval == 0){ // Fled
                Console.Clear(); // Clear the console and write the UI
                WriteStatus();
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Fleeing may not be the most valorous, but you live another day").AllowEmpty());
                return 0;
            }
            else if(retval == 1){ // NPC Fled
                Console.Clear(); // Clear the console and write the UI
                WriteStatus();
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("A sudden quiet overcomes the ship, your opponet fled, leaving you alone once again").AllowEmpty());
                return 0;
            }
            else if (retval == 2){ // Won
                Console.Clear(); // Clear the console and write the UI
                WriteStatus();
                AnsiConsole.Write(new Markup("[green]Victory![/]\n"));
                mainPlayer.combatRewards(difficulty);
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
                return 0;
            }
            else{ // Died
                Console.Clear();
                AnsiConsole.Write(new Markup("[red]GAME OVER[/]\n"));
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to return to the main menu...........").AllowEmpty());
                return -1;
            }
        }

        private void TestCombat(){
            Combat tCombat = new Combat(0); // Create a new very easy combat
            int retval = tCombat.Fight(mainPlayer);
        }

        private void DebugMenu(){
            string selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Debug:")
            .PageSize(10)
            .AddChoices(new[] {"Damage Ship", "Damage Hull", "Add Cargo", "Remove Cargo", "Change Systems", "Edit Money", "Combat Test", "View Log", "Return"}));

            switch(selection){
                case "Damage Ship":
                    int damage = AnsiConsole.Prompt(new TextPrompt<int>("Enter damage amount: "));
                    mainPlayer.PShip.TakeDamage(damage);
                    Console.Clear(); // Clear the console and write the UI
                    WriteStatus();
                    DebugMenu();
                break;
                case "Damage Hull":
                    int hullDamage = AnsiConsole.Prompt(new TextPrompt<int>("Enter hull damage amount: "));
                    mainPlayer.PShip.Hull.Hullval -= hullDamage;
                    Console.Clear(); // Clear the console and write the UI
                    WriteStatus();
                    DebugMenu();
                break;
                case "Edit Money":
                    int money = AnsiConsole.Prompt(new TextPrompt<int>("Enter new amount of money"));
                    mainPlayer.Money = money;
                    Console.Clear(); // Clear the console and write the UI
                    WriteStatus();
                    DebugMenu();
                break;
                case "Add Cargo":
                break;
                case "Remove Cargo":
                break;
                case "Change Systems":
                break; 
                case "Combat Test":
                    TestCombat();
                break;
                case "View Log":
                    mainPlayer.displayLog();
                    Console.Clear(); // Clear the console and write the UI
                    WriteStatus();
                    DebugMenu();
                break;
                case "Return":
                    OptionsMenu();
                break;
            }
        }
    }
}