using System.Configuration;
using System;
using Basiverse;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;
using System.Threading;

namespace Basiverse
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup();
            PrintHeader();
            MainMenu();
            AnsiConsole.Clear();
        }

        static void MainMenu(){
            // Create the Save/Load menu
            PrintHeader(); 
            bool debug = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["debugMode"]);
            string selection;

            if(debug){
                selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(4)
                    .AddChoices(new[] { "New", "Load",  "Quit", "Debug" }));
            }
            else{
                selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(4)
                    .AddChoices(new[] { "New", "Load",  "Quit"}));
            }         

            switch(selection){
                case "New":
                    NewGameMenu();
                break;
                case "Load":
                    LoadGameMenu();
                break;
                case "Quit":
                    return;
                case "Debug":
                    DebugMenu();
                break;
            }
        }

        static void NewGameMenu(){
            AnsiConsole.Clear();
            PrintHeader();
            Player mainPlayer = new Player(); // Create our new player and ship
            mainPlayer.PShip = new Ship();
            mainPlayer.Name = AnsiConsole.Ask<string>("Please enter your name:"); 
            AnsiConsole.Markup("Welcome {0}", mainPlayer.Name);
            System.Threading.Thread.Sleep(1000);

            AnsiConsole.Clear();
            PrintHeader();
            if(AnsiConsole.Confirm("Would you like to name your ship?")){ // Naming Check
                mainPlayer.PShip.Name = AnsiConsole.Ask<string>("Please enter the new name:");
            }
            else{
                AnsiConsole.MarkupLine("Okay");
            }

            mainPlayer.PShip.DisplayData(); // Data dump


            Game NewGame = AnsiConsole.Status().Start("Starting New Game", ctx => {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));
                AnsiConsole.MarkupLine("Setting up...");
                Game NewGame = new Game(mainPlayer);
                // Update the status and spinner
                ctx.Status("Generating up the Map");
                MapGen mapHelper = new MapGen();
                ctx.Status("Assigning Map to player");
                mainPlayer.PMap = mapHelper.Generate(false);
                mainPlayer.PLoc = mainPlayer.PMap.AllNodes[0]; // Put the player at node 0
                ctx.Status("Good Luck!");
                return NewGame;
            });
            NewGame.Start(); // Can't have this nested in the status

        }

        static void LoadGameMenu(){
            AnsiConsole.Clear();
            PrintHeader();
            Loader MainLoad = new Loader(); // Create the Loader
            // Check that we have at a save
            if(!MainLoad.CheckSave()){
                var tmp = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]No save data, press any key to continue[/]")
                    .AllowEmpty());
                MainMenu();
            }
            else{
                string name = MainLoad.GetName();
                AnsiConsole.MarkupLine("A save exists with the player name {0}", name);
                if (!AnsiConsole.Confirm("Load Game?"))
                {
                    MainMenu();
                }
                else{
                    // TODO actually load saves and pass into the game   
                }
            }
        }

        static void PrintHeader(){
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("  ____            _         ____   _____        __   ___  __ ");
            AnsiConsole.MarkupLine(" |  _ \\          (_)       / __ \\ / ____|      /_ | / _ \\/_ |");
            AnsiConsole.MarkupLine(" | |_) | __ _ ___ _  ___  | |  | | (___   __   _| || | | || |");
            AnsiConsole.MarkupLine(" |  _ < / _` / __| |/ __| | |  | |\\___ \\  \\ \\ / / || | | || |");
            AnsiConsole.MarkupLine(" | |_) | (_| \\__ \\ | (__  | |__| |____) |  \\ V /| || |_| || |");
            AnsiConsole.MarkupLine(" |____/ \\__,_|___/_|\\___|  \\____/|_____/    \\_/ |_(_)___/ |_|");
             AnsiConsole.MarkupLine("");
        }

        static void TestDataAcess(){
            AnsiConsole.Markup("Creating Test Player");
            Player mainPlayer = new Player(); // Create our test player and ship
            mainPlayer.Name = "Testing player load and save";
            mainPlayer.PShip = new Ship();
            mainPlayer.PShip.Name = "Testing ship load and save";
            AnsiConsole.MarkupLine("Creating new Game");
            Game NewGame = new Game(mainPlayer);
            AnsiConsole.MarkupLine("Dumping Original data");
            NewGame.Dump();
            Player LoadPlayer = new Player(); // Create load player
            LoadPlayer = NewGame.TestSave();
            AnsiConsole.MarkupLine("Loading, dumping loaded Data");
            Game TestGame = new Game(LoadPlayer);
            TestGame.Dump();
            var tmp = AnsiConsole.Prompt(
                new TextPrompt<string>("Press any key to continue")
                .AllowEmpty());
            DebugMenu();
        }

        static void DebugMenu(){
            AnsiConsole.Clear();
            ItemHelper helper1 = new ItemHelper();
            MapGen mapHelper = new MapGen();
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Debug Menu")
                .PageSize(7)
                .AddChoices(new[] { "Test Data Acess", "Run Object Bin Creator", "Test Object Load", "Generate Map", "Test Map Load", "Return" }));
            switch(selection){
                case "Test Data Acess":
                    TestDataAcess();
                break;
                case "Run Object Bin Creator":
                    helper1.CreateObjects();
                break;
                case "Test Object Load":
                    helper1.LoadObjTest();
                    DebugMenu();
                break;
                case "Generate Map":
                    mapHelper.Generate(true);
                    DebugMenu();
                break;
                case "Test Map Load":
                    mapHelper.CheckBin();
                    DebugMenu();
                break;
                case "Return":
                    MainMenu();
                break;
            }
        }

        static void Startup(){
            try{
                AnsiConsole.Status().Start("Beginning Game Initialization.....", ctx => { // Using context to disp init data
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    ctx.Status("Generating bin files");
                    ItemHelper helper1 = new ItemHelper();
                    helper1.CreateObjects();
                    ctx.Status("Bin filegen complete");
                    ctx.Status("Creating Save Directory if needed");
                    string loc = Directory.GetCurrentDirectory() + "\\Save\\";
                    if(Directory.Exists(loc)){
                        AnsiConsole.Markup($"Save folder already exists");
                    }
                    else{
                        DirectoryInfo di = Directory.CreateDirectory(loc);
                        AnsiConsole.Markup("The directory was created successfully at {0}.", Directory.GetCreationTime(loc));

                    }
                    Thread.Sleep(500);
            });
            }
            catch (Exception e){
                AnsiConsole.WriteException(e);
            }
        }
    }
}
