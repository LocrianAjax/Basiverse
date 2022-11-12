using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class MiningMinigame{
        public Asteroid MiningAsteroid; // Using a default size of 6
        public int Size {get; set;}
    
        public MiningMinigame(){
            Size = 6;
            MiningAsteroid = new Asteroid(Size);
        }

        public MiningMinigame(int inSize){
            Size = inSize;
            MiningAsteroid = new Asteroid(Size);
        }

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
            int selected = 0;
            MiningAsteroid.Tiles[selected].isSelected = true;
            MiningAsteroid.Draw();
            AnsiConsole.Cursor.Hide();
            //  Now that we're set up, start the loop
            while(true){
                DrawMiningAssist();

                ConsoleKey input = TimedReader.ReadKey(250);
                MiningAsteroid.Tiles[selected].isSelected = false;
                if(input == ConsoleKey.DownArrow){
                    selected += Size;
                }
                if(input == ConsoleKey.UpArrow){
                    selected -= Size;
                }
                if(input == ConsoleKey.LeftArrow){         
                    selected -= 1;
                }
                if(input == ConsoleKey.RightArrow){
                    selected += 1;
                }

                // Validate selection
                if(selected > (Size * Size) - 1){
                    selected = (Size * Size) - 1;
                }
                else if(selected < 0){
                    selected = 0;
                }
                
                // Then draw
                MiningAsteroid.Tiles[selected].isSelected = true;
                MiningAsteroid.Draw();
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