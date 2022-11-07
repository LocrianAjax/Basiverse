using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class MiningMinigame{
        public Crosshair MiningCrosshair = new Crosshair();
        public Asteroid MiningAsteroid = new Asteroid();
    
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
                    var LaserInit = ctx.AddTask("[green]CALIBRATING LASERS FOR MINING FREQUENCIES.......[/]");
                    while(MiningInit.IsFinished && !LaserInit.IsFinished){
                        LaserInit.Increment(randProg.Next(1,10));
                        Thread.Sleep(75);
                    }
                });
                Thread.Sleep(300);
                AnsiConsole.Clear();
                DrawMiningAssist();
                AnsiConsole.Cursor.Hide();
                MiningCrosshair.Draw();
                AnsiConsole.Cursor.Hide();
                //  Now that we're set up, start the loop
                while(true){
                    DrawMiningAssist();
                    MiningCrosshair.Draw();

                    ConsoleKey input = TimedReader.ReadKey(250);
                    if(input == ConsoleKey.DownArrow){
                        MiningCrosshair.InertiaY += 1;
                        MiningCrosshair.moveDown();
                    }
                    if(input == ConsoleKey.UpArrow){
                        MiningCrosshair.InertiaY -= 1;
                        MiningCrosshair.moveUp();
                    }
                    if(input == ConsoleKey.LeftArrow){
                        MiningCrosshair.InertiaX -= 1;
                        MiningCrosshair.moveLeft();
                    }
                    if(input == ConsoleKey.RightArrow){
                        MiningCrosshair.InertiaX += 1;
                        MiningCrosshair.moveRight();
                    }
                    AnsiConsole.Cursor.Hide();
                }
            }
    
            public void DrawMiningAssist(){
                AnsiConsole.Cursor.Hide();
                AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row
                AnsiConsole.Write("                                                                                                            ");
                AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row
                AnsiConsole.Markup($"[green][/]");
                AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
                AnsiConsole.Write("                                                                                                            ");
                AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
                AnsiConsole.Markup($"[green][/]");
        }
    }

}