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
            NavScreen.AddColumns("Local System", ""); 
            NavScreen.AddRow(mainPlayer.PLoc.Name, mainPlayer.PLoc.Description);

            Table PoiScreen = new Table();
            PoiScreen.AddColumns("Point of Intrest", "");
            if(mainPlayer.PLoc.Interests != null){
                foreach(PointofInterest poi in mainPlayer.PLoc.Interests){
                PoiScreen.AddRow($"{poi.Name}", $"{poi.Description}");
                PoiScreen.AddEmptyRow();
                }
            }

            // Add data for status table
            Table StatusScreen = new Table();
            StatusScreen.AddColumn(mainPlayer.PShip.Name);
            // Add rows for Hull/Heat/Shield

            // Shield Status
            double shieldSw = Math.Floor(mainPlayer.PShip.ShieldVal());
            if( shieldSw >= 75){
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [green]{shieldSw}[/]%"));
            }
            else if(shieldSw < 75 && mainPlayer.PShip.ShieldVal() > 25 ){
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [yellow]{shieldSw}[/]%"));
            }
            else if(shieldSw == 0){
                StatusScreen.AddRow(new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [darkorange]{shieldSw}[/]%"));
            }

            // Hull Status
            double hullSw = Math.Floor(mainPlayer.PShip.HullVal());
            if(hullSw >= 75){
                StatusScreen.AddRow(new Markup($"Hull Integrity: [green]{hullSw}[/]%"));
            }
            else if (hullSw <= 25){
                StatusScreen.AddRow(new Markup($"Hull Integrity: [red]{hullSw}[/]%"));
            }
            else{
                StatusScreen.AddRow(new Markup($"Hull Integrity: [yellow]{hullSw}[/]%"));
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
            MainScreen.AddRow(PoiScreen);
            MainScreen.Expand();
            MainScreen.Border(TableBorder.Ascii);
            AnsiConsole.Write(MainScreen);
        }
        
        private void MainActionMenu(){ // Menu for Main Game actions

             // Set up choices
            string choices = "Jump|";
            // Check and see if we have any stations
            if(mainPlayer.PLoc.Interests != null){
                foreach(PointofInterest temp in mainPlayer.PLoc.Interests){
                    if(temp.Type == 2){
                        choices += "Dock|";
                        break;
                    }
                }
            }

            choices += "Detailed Report|Options";
            // Then split on | to create dynamic menu
            string[] options = choices.Split('|');
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Actions:")
                .PageSize(4)
                .AddChoices(options));
            
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
                case "Dock":
                    DockMenu();
                break;
            }
        }

        private void JumpMenu(){
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
                MainActionMenu();
            }
            else{
                foreach(Location temp in mainPlayer.PLoc.NearbyNodes){
                    if(destination == temp.Name){
                        mainPlayer.PLoc = temp;
                        mainPlayer.JumpLog += 1;
                        // This is just for show
                        AnsiConsole.Progress().HideCompleted(false).Start(ctx => {
                            var warmEngine = ctx.AddTask("[red]Warming warp engine up to operating temperature[/]");
                            var jumping = ctx.AddTask("[green]Jumping[/]");
                            while(!warmEngine.IsFinished){
                                warmEngine.Increment(3);
                                Thread.Sleep(20);
                            }
                            while(!jumping.IsFinished){
                                jumping.Increment(10);
                                Thread.Sleep(100);
                            }
                        });
                        // Back to real code
                        Start();
                    }
                }
            }
            
        }
        private void OptionsMenu(){
            string opts = "Settings|Return|Save|Save and Quit|Quit without Saving";
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
                    MainActionMenu();
                break;
                case "Save":
                    SaveGame();
                    MainActionMenu();
                break;
                case "Save and Quit":
                    SaveGame();
                    System.Environment.Exit(0);
                break;
                case "Debug":
                    DebugMenu();
                break;
                case "Quit without Saving":
                   System.Environment.Exit(0);
                break;
            }
        }

        private void DetailedReportMenu(){
            AnsiConsole.Clear();
            // Add data for Detailed Ship Report
            /*
                SYSTEM  |   REPORT
                Shield  |   info
                etc.    |   etc.
            */
            Table ReportScreen = new Table();
            ReportScreen.Title = new TableTitle($"{mainPlayer.PShip.Name} DETAILED SYSTEMS REPORT. CHASSIS: {mainPlayer.PShip.Chassis} CAPITAN: {mainPlayer.Name}");
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
            double shieldSw = Math.Floor(mainPlayer.PShip.ShieldVal());
            if(shieldSw >= 75){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"[cyan]ONLINE[/] - Strength: [green]{shieldSw}[/]%"));
            }
            else if(shieldSw < 75 && mainPlayer.PShip.ShieldVal() > 25 ){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"[cyan]ONLINE[/] - Strength: [yellow]{shieldSw}[/]%"));
            }
            else if(shieldSw == 0){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"[red][rapidblink]OFFLINE[/][/]"));
            }
            else{
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Shield.Name}"), new Markup($"[cyan]ONLINE[/] - Strength: [orange]{shieldSw}[/]%"));
            }

            // Hull Status
            double hullSw = Math.Floor(mainPlayer.PShip.HullVal());
            if(hullSw >= 75){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Hull.Name}"), new Markup($"Integrity: [green]{hullSw}[/]%"));
            }
            else if (hullSw <= 25){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Hull.Name}"), new Markup($"Integrity: [red]{hullSw}[/]%"));
            }
            else{
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Hull.Name}"), new Markup($"Integrity: [yellow]{hullSw}[/]%"));
            }
            
            // Armor Data
            if(mainPlayer.PShip.Armor.Name != "None"){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Armor.Name}"), new Markup($"Damage Mitigation: {mainPlayer.PShip.Armor.ArmorValue}"));
            }
            else{
                ReportScreen.AddRow(new Markup($"No Armor installed"));
            }

            // Heat Status
            string OnlineStat;
            if(mainPlayer.PShip.Heatsink.IsActive){
                OnlineStat = "[green]Active Cooling[/]";
            }
            else{
                OnlineStat = "[red]Passive Cooling[/]";
            }
            if(mainPlayer.PShip.HeatVal() <= 25){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Heatsink.Name}"), new Markup($"{OnlineStat} Heat Soak - [green]{mainPlayer.PShip.HeatVal()}[/]%"));
            }
            else if(mainPlayer.PShip.HeatVal() <= 75 && mainPlayer.PShip.HeatVal() > 25){
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Heatsink.Name}"), new Markup($"{OnlineStat} Heat Soak - [yellow]{mainPlayer.PShip.HeatVal()}[/]%"));
            }
            else{
                ReportScreen.AddRow(new Markup($"{mainPlayer.PShip.Heatsink.Name}"), new Markup($"{OnlineStat} Heat Soak - [red]{mainPlayer.PShip.HeatVal()}[/]%"));
            }

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
            Start();
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
            SettingsMenu();
        }

        private void RenameShip(){
            bool res = false;
            while(res != true){
                string Name = AnsiConsole.Ask<string>("Please enter your ship's new name:");
                res = mainPlayer.PShip.Rename(Name);
            }
            Console.Clear(); // Clear the console and write the UI
            WriteStatus();
            SettingsMenu();
        }

        private void SaveGame(){
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
            return;
        }

        private void DockMenu(){
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
                MainActionMenu();
            }
            else{
                foreach(Station tmp in mainPlayer.PLoc.Stations){ // Not effcient, but we're going to a max of 5 things so no biggie
                    if(tmp.Name == destination){
                        tmp.Dock(mainPlayer);
                        Start();
                    }
                }
            }
        }

        private void CombatMenu(){ // Combat loop: 2 actions then check heat, and check for death.
            // TODO
        }

        private void DebugMenu(){
            string selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Debug:")
            .PageSize(7)
            .AddChoices(new[] {"Damage Ship", "Damage Hull", "Add Cargo", "Remove Cargo", "Change Systems", "Edit Money","Return"}));

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
                case "Return":
                    OptionsMenu();
                break;
            }
        }
    }
}