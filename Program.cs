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
            AnsiConsole.Cursor.Hide();
            Startup();
            PrintHeader();
            StartupInfo();
            MainMenu();
            AnsiConsole.Clear();
        }

        static void MainMenu(){
            while(true){
                // Create the Save/Load menu
                PrintHeader();
                Loader MainLoad = new Loader(); // Create the Loader
                // Set up choices
                string choices = "New|";
                if(MainLoad.CheckSave()){
                    choices += "Load|";
                }
                if(Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["debugMode"])){
                    choices += "Debug|";
                }
                choices += "Quit";
                // Then split on | to create dynamic menu
                string[] options = choices.Split('|');
                string selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(4)
                    .AddChoices(options));
                        
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
        }

        static void NewGameMenu(){
            AnsiConsole.Clear();
            PrintHeader();
            Player mainPlayer = new Player(); // Create our new player and ship
            mainPlayer.Money += 100;
            mainPlayer.PShip = new Ship();
            mainPlayer.Name = AnsiConsole.Ask<string>("Please enter your name:"); 
            AnsiConsole.Markup("Welcome {0}", mainPlayer.Name);
            System.Threading.Thread.Sleep(1000);

            AnsiConsole.Clear();
            PrintHeader();
            if(AnsiConsole.Confirm("Would you like to name your ship?")){ // Naming Check
                mainPlayer.PShip.Name = AnsiConsole.Ask<string>("Please enter the name: The ");
            }
            else{
                AnsiConsole.MarkupLine("Okay");
            }

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
                mainPlayer.logStart(); // Log the starting location
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
                if (!AnsiConsole.Confirm("Load Game?")){
                    MainMenu();
                }
                else{
                    Loader gameLoad = new Loader();
                    Player loadPlayer = gameLoad.LoadSave();
                    Game loadGame = AnsiConsole.Status().Start("Loading Game", ctx => {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("green"));
                        Game loadgame = new Game(loadPlayer);
                        ctx.Status("Good Luck!");
                        return loadgame;
                    }); 
                    loadGame.Start();
                }
            }
        }

        static void PrintHeader(){
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Basic OS v1.01"));
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

        static void TestNPCCreation(){
            Generator testGen = new Generator();
            NPC testNPC1 = testGen.GenerateCombatNPC("Tester", 1, 1);
            AnsiConsole.WriteLine("Test 1 Info");
            testNPC1.cShip.DisplayData();
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
            NPC testNPC2 = testGen.GenerateCombatNPC("Tester", 2, 1);
            AnsiConsole.WriteLine("Test 2 Info");
            testNPC2.cShip.DisplayData();
            tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
            NPC testNPC3 = testGen.GenerateCombatNPC("Tester", 3, 1);
            AnsiConsole.WriteLine("Test 3 Info");
            testNPC3.cShip.DisplayData();
            tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
            NPC testNPC4 = testGen.GenerateCombatNPC("Tester", 4, 1);
            AnsiConsole.WriteLine("Test 4 Info");
            testNPC4.cShip.DisplayData();
            tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
            NPC testNPC5 = testGen.GenerateCombatNPC("Tester", 5, 1);
            AnsiConsole.WriteLine("Test 5 Info");
            testNPC5.cShip.DisplayData();
            tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue").AllowEmpty());
        }

        static void DebugMenu(){
            AnsiConsole.Clear();
            ItemHelper helper1 = new ItemHelper();
            MapGen mapHelper = new MapGen();
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Debug Menu")
                .PageSize(8)
                .AddChoices(new[] { "Test Data Acess", "Run Object Bin Creator", "Test Object Load", "Generate Map Test", "Delete Save", "Generate NPC Test", "Docking Test", "Return" }));
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
                case "Generate Map Test":
                    mapHelper.Generate(true);
                    new TextPrompt<string>("Press any key to continue...").AllowEmpty();
                    DebugMenu();
                break;
                case "Delete Save":
                    Deleter tempDel = new Deleter();
                    tempDel.DeleteData();
                    DebugMenu();
                break;
                case "Generate NPC Test":
                    TestNPCCreation();
                break;
                case "Docking Test":
                    TestDocking();
                break;
                case "Return":
                    MainMenu();
                break;
            }
        }

        static void TestDocking(){
            DockingMinigame tester = new DockingMinigame();
            tester.StartMinigame();
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

        static void StartupInfo(){ // Info on program start
            LoreReader newReader = new LoreReader();
            var tmp = AnsiConsole.Prompt(
                new TextPrompt<string>(newReader.GetStartupLines())
                .AllowEmpty());
        }

        static void GameIntro(){ // Before actually starting the game this is our intro screen
            LoreReader newReader = new LoreReader();
            var tmp = AnsiConsole.Prompt(
                new TextPrompt<string>(newReader.GetNewGameLines())
                .AllowEmpty());
        }
    }
}
