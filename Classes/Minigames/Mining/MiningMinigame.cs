using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class MiningMinigame{
        public Asteroid MiningAsteroid = new Asteroid(); // Using a default size of 6
    
        public bool StartMinigame(){
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            // Draw a little fake Init screen
            var randProg = new Random();
            AnsiConsole.Progress().HideCompleted(true).Start(ctx => {
                var MiningInit = ctx.AddTask("[green]BRINGING MINING SUB-SYSTEMS ONLINE.......[/]");
                while(!MiningInit.IsFinished){
                    MiningInit.Increment(randProg.Next(1,10));
                    Thread.Sleep(75);
                }
                var LaserInit = ctx.AddTask("[green]TUNING LASERS TO MINING FREQUENCIES.......[/]");
                while(MiningInit.IsFinished && !LaserInit.IsFinished){
                    LaserInit.Increment(randProg.Next(1,10));
                    Thread.Sleep(75);
                }
            });
            
            Thread.Sleep(300);
            AnsiConsole.Clear();
            DrawMiningAssist();
            AnsiConsole.Cursor.Hide();
            MiningAsteroid.Draw();
            AnsiConsole.Cursor.Hide();
            //  Now that we're set up, start the loop
            while(true){
                DrawMiningAssist();

                ConsoleKey input = TimedReader.ReadKey(250);
                if(input == ConsoleKey.DownArrow){

                }
                if(input == ConsoleKey.UpArrow){

                }
                if(input == ConsoleKey.LeftArrow){

                }
                if(input == ConsoleKey.RightArrow){

                }
                AnsiConsole.Cursor.Hide();
            }
        }
    
        public void DrawMiningAssist(){
            // Use ▀ char in green and red to show the heat
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row 
            AnsiConsole.Write("                                                                                                            ");
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row
            AnsiConsole.Markup($"[green]ASTEROID STRUCTURAL INTEGRITY[/]");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
            AnsiConsole.Write("                                                                                                            ");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
            // Draw the "Health Indicator"
            int RedSquares = (MiningAsteroid.MaxHealth / 4) - (MiningAsteroid.CurrHealth / 4);
            int GreenSquares = (MiningAsteroid.MaxHealth / 4) - RedSquares;
            for(int i = 0; i < GreenSquares; i++){
                AnsiConsole.Markup($"[green]▀[/]");
            }
            for(int j = 0; j < RedSquares; j++){
                AnsiConsole.Markup($"[red]▀[/]");
            }
        }
    }

}