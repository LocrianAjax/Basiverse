using System;
using Basiverse;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace Basiverse
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup();
            // Create the intro
            PrintHeader();
            Console.Write("\nBasicOs Loading");
            for(int i = 0; i < 10; i++){System.Threading.Thread.Sleep(100); Console.Write(".");}
            Console.WriteLine("\nWelcome USER");
            Console.Clear();
            MainMenu();
            Console.Clear();
        }

        static void MainMenu(){
            // Create the Save/Load menu
            while(true){  
                PrintHeader();          
                Console.Write("\n\n1. New Game\n2. Load Game\n3. Quit\n4. Debug Menu\nPlease make a selection -: ");
                string selection = Console.ReadLine();
                switch(selection){
                    case "1":
                        NewGameMenu();
                    break;
                    case "2":
                        LoadGameMenu();
                    break;
                    case "3":
                        return;
                    case "4":
                        DebugMenu();
                    break;
                    default:
                    break;
                }
                Console.Clear();
            }
        }

        static void NewGameMenu(){
            Console.Clear();
            PrintHeader();
            Player mainPlayer = new Player(); // Create our new player and ship
            mainPlayer.PShip = new Ship();
            Console.Write("\nPlease enter your name: ");
            mainPlayer.Name = Console.ReadLine(); 
            Console.WriteLine("\nWelcome {0}", mainPlayer.Name);
            System.Threading.Thread.Sleep(1000);
            
            string resp = "";
            while((resp != "y") && (resp != "n")){
                Console.Clear();
                PrintHeader();
                Console.Write("\nWould you like to name your ship? y/n: ");
                resp = Console.ReadLine().ToLower();
                switch(resp){
                    case "y":
                        Console.Write("Please enter the new name: ");
                        mainPlayer.PShip.Name = Console.ReadLine();
                    break;
                    case "n":
                        Console.WriteLine("Okay");
                    break;
                }
            }
            mainPlayer.PShip.DisplayData();
            Console.WriteLine("\n Good luck!");
            Game NewGame = new Game(mainPlayer);
            NewGame.Dump();
            Console.ReadKey();
            // Game.Start();
        }

        static void LoadGameMenu(){
            Console.Clear();
            PrintHeader();
            Loader MainLoad = new Loader(); // Create the Loader
            // Check that we have at least 1 save
            if(!MainLoad.CheckSave(1) && !MainLoad.CheckSave(2) && !MainLoad.CheckSave(3)){
                Console.WriteLine("\nNo saves available, press any key to continue");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\nPlease select a save number to load or q to return to the menu -\n");
            if(MainLoad.CheckSave(1)){
                Console.WriteLine("Save 1: {0}", MainLoad.GetName(1));
            }
            if(MainLoad.CheckSave(2)){
                Console.WriteLine("Save 2: {0}", MainLoad.GetName(2));
            }
            if(MainLoad.CheckSave(3)){
               Console.WriteLine("Save 3: {0}", MainLoad.GetName(3));
            }
            Console.Write("\n: ");
            Console.ReadLine();
            // TODO actually load saves and pass into the game
        }

        static void PrintHeader(){
            Console.Clear();
            Console.WriteLine("  ____            _         ____   _____        __   ___  __ ");
            Console.WriteLine(" |  _ \\          (_)       / __ \\ / ____|      /_ | / _ \\/_ |");
            Console.WriteLine(" | |_) | __ _ ___ _  ___  | |  | | (___   __   _| || | | || |");
            Console.WriteLine(" |  _ < / _` / __| |/ __| | |  | |\\___ \\  \\ \\ / / || | | || |");
            Console.WriteLine(" | |_) | (_| \\__ \\ | (__  | |__| |____) |  \\ V /| || |_| || |");
            Console.WriteLine(" |____/ \\__,_|___/_|\\___|  \\____/|_____/    \\_/ |_(_)___/ |_|");
        }

        static void TestDataAcess(){
            Console.WriteLine("Creating Test Player");
            Player mainPlayer = new Player(); // Create our test player and ship
            mainPlayer.Name = "Testing player load and save";
            mainPlayer.PShip = new Ship();
            mainPlayer.PShip.Name = "Testing ship load and save";
            Console.WriteLine("Creating new Game");
            Game NewGame = new Game(mainPlayer);
            Console.WriteLine("\nDumping Original data");
            NewGame.Dump();
            Console.WriteLine("\n\n");
            Player LoadPlayer = new Player(); // Create load player
            LoadPlayer = NewGame.TestSave();
            Console.WriteLine("\nLoading, dumping loaded Data");
            Game TestGame = new Game(LoadPlayer);
            TestGame.Dump();
            Console.ReadKey();
        }

        static void DebugMenu(){
            ItemHelper helper1 = new ItemHelper();
            MapGen mapHelper = new MapGen();
            while(true){  
                PrintHeader();
                Console.WriteLine("DEBUG MENU");
                Console.Write("\n\n1. Test Data Acess\n2. Run Object Bin Creator\n3. Load Object Test\n4. Generate Map\n5. Test Map Load\n6. Quit\n Please make a selection -: ");
                string selection = Console.ReadLine();
                switch(selection){
                    case "1":
                        TestDataAcess();
                    break;
                    case "2":
                        helper1.CreateObjects();
                    break;
                    case "3":
                        helper1.LoadObjTest();
                    break;
                    case "4":
                        mapHelper.Generate(true);
                    break;
                    case "5":
                        mapHelper.CheckBin();
                    break;
                    case "6":
                        return;
                    default:
                        break;
                }
            }
        
        }

        static void Startup(){
            try{
                Console.WriteLine("Beginning Game Initialization.....");
                Console.WriteLine("Generating bin files");
                ItemHelper helper1 = new ItemHelper();
                helper1.CreateObjects();
                Console.WriteLine("Complete");
                Console.WriteLine("Creating Saves Directory if needed");
                string [] Locations = new string[3];
                Locations[0] = Directory.GetCurrentDirectory() + "\\Saves\\save1\\";
                Locations[1] = Directory.GetCurrentDirectory() + "\\Saves\\save2\\";
                Locations[2] = Directory.GetCurrentDirectory() + "\\Saves\\save3\\";
                foreach(string loc in Locations){
                    if(Directory.Exists(loc)){
                        Console.WriteLine($"Save folder at {loc} already exists");
                    }
                    else{
                        DirectoryInfo di = Directory.CreateDirectory(loc);
                        Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(loc));

                    }
                }
            }
            catch (Exception e){
                Console.WriteLine("Setup failed: {0}", e.ToString());
            }
        }

    }
}
