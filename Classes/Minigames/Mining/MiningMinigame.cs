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
            MiningAsteroid.GenerateTiles();
        }

        public MiningMinigame(int inSize){
            Size = inSize;
            MiningAsteroid = new Asteroid(Size);
            MiningAsteroid.GenerateTiles();
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
            
            // Set up the inital screen
            Thread.Sleep(300);
            AnsiConsole.Clear();
            DrawMiningAssist();
            AnsiConsole.Cursor.Hide();
            int selected = 0;
            MiningAsteroid.Tiles[selected].isSelected = true;
            MiningAsteroid.DrawAll();
            AnsiConsole.Cursor.Hide();

            //  Now that we're set up, start the loop
            while(true){
                DrawMiningAssist();
                ConsoleKey input = TimedReader.ReadKey(250);
                int oldSelected = selected;
                if(input == ConsoleKey.DownArrow){
                    if((selected + Size) > (Size * Size) - 1) {} // Do nothing
                    else{selected += Size;}
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.UpArrow){
                    if((selected - Size) < 0) {} // Do nothing
                    else{selected -= Size;}
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.LeftArrow){       
                    selected -= 1;
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.RightArrow){
                    selected += 1;
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.Spacebar){
                    // Damage the tile and change it's version
                    // TODO: That
                    // For now just highlight it and remove health
                    HightlightAndDamage(selected);
                }

                // Check for game over case
                if(MiningAsteroid.CurrHealth <= 0){
                    DrawMiningAssist();
                    AnsiConsole.Cursor.SetPosition(0,3);
                    AnsiConsole.Write("Asteroid disintegration!");
                    AnsiConsole.Cursor.SetPosition(0,5);
                    var tmp = AnsiConsole.Prompt(
                    new TextPrompt<string>("Press any key to continue")
                    .AllowEmpty());
                    return false;
                }
            }
        }

        public void HightlightAndDamage(int inSelection){
            MiningAsteroid.Tiles[inSelection].isSelected = false;
            MiningAsteroid.Tiles[inSelection].isHighlighted = true;
            MiningAsteroid.DrawAt(inSelection);
            Thread.Sleep(100);
            MiningAsteroid.Tiles[inSelection].Damage(); // TODO: Check for loot and add something for that here.
            MiningAsteroid.Tiles[inSelection].isHighlighted = false;
            MiningAsteroid.Tiles[inSelection].isSelected = true;
            MiningAsteroid.DrawAt(inSelection);
            MiningAsteroid.CurrHealth -= 5;
        }

        public int ValidateAndDraw(int inSelection, int oldSelection){
            MiningAsteroid.Tiles[oldSelection].isSelected = false;
            MiningAsteroid.DrawAt(oldSelection);
            int selected = inSelection;
            // Validate selection
            if(selected > (Size * Size) - 1){
                selected = (Size * Size) - 1;
            }
            else if(selected < 0){
                selected = 0;
            }

            // Then draw
            MiningAsteroid.Tiles[selected].isSelected = true;
            MiningAsteroid.DrawAt(selected);
            AnsiConsole.Cursor.Hide();
            return selected;
        }
    
        public void DrawMiningAssist(){
            // Use ▀ char in green and red to show the heat
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row 
            AnsiConsole.Write("                                         ");
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row
            AnsiConsole.Markup($"[green]ASTEROID STRUCTURAL INTEGRITY[/]");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
            AnsiConsole.Write("                                         ");
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