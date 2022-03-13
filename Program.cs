using System;
using Basiverse;
using System.Collections.Generic;

namespace Basiverse
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Create the intro
            PrintHeader();
            Console.Write("\nBasicOs Loading");
            for(int i = 0; i < 10; i++){System.Threading.Thread.Sleep(400); Console.Write(".");}
            Console.WriteLine("\nWelcome USER");
            Console.Clear();
            MainMenu();
            Console.Clear();
        }

        static void MainMenu(){
            // Create the Save/Load menu
            while(true){  
                PrintHeader();          
                Console.Write("\nPlease make a selection - \n1. New Game\n2. Load Game\n3. Quit\n4. Debug Menu\n: ");
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
                        TestDataAcess();
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

    }
}
