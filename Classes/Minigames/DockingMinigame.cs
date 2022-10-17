using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class DockingMinigame{
        // TODO: This
        public Target DockingTarget;
        public Crosshair DockingCrosshair;

        public DockingMinigame(){ // Default
            DockingCrosshair = new Crosshair();
            DockingTarget = new Target();
        }

        public DockingMinigame(int TargetX, int TargetY){ // Specified locations
            DockingCrosshair = new Crosshair();
            DockingTarget = new Target(TargetX, TargetY);
        }

        public void DrawDriftAssist(){
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Cursor.SetPosition(0,0); // Reset to the top row
            AnsiConsole.Write("                                                                                                            ");
            AnsiConsole.Cursor.SetPosition(0,0); // Reset to the top row
            AnsiConsole.Markup($"[green]DOCKING ASSIST - YAW DRIFT: {DockingCrosshair.InertiaX} PITCH DRIFT: {DockingCrosshair.InertiaY}[/]");
        }

        public bool StartMinigame(){
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            // Draw a little fake Init screen
            var randProg = new Random();
            AnsiConsole.Progress().HideCompleted(true).Start(ctx => {
                var DockingInit = ctx.AddTask("[green]Bringing Docking Systems online......[/]");
                while(!DockingInit.IsFinished){
                    DockingInit.Increment(randProg.Next(1,10));
                    Thread.Sleep(20);
                }
            });
            Thread.Sleep(300);
            AnsiConsole.Clear();
            DrawDriftAssist();
            DockingTarget.Draw();
            AnsiConsole.Cursor.Hide();
            DockingCrosshair.Draw();
            AnsiConsole.Cursor.Hide();
            //  Now that we're set up, start the loop
            while(true){
                DrawDriftAssist();
                DockingTarget.Drift(DockingCrosshair.InertiaX, DockingCrosshair.InertiaY); // Drift with inertia
                DockingCrosshair.Draw();

                ConsoleKey input = Console.ReadKey().Key; // Get input and move the target the opposite direction
                if(input == ConsoleKey.DownArrow){
                    DockingCrosshair.InertiaY += 1;
                    DockingTarget.moveUp();
                    }
                if(input == ConsoleKey.UpArrow){
                    DockingCrosshair.InertiaY -= 1;
                    DockingTarget.moveDown();
                    }
                if(input == ConsoleKey.LeftArrow){
                    DockingCrosshair.InertiaX -= 1;
                    DockingTarget.moveRight();
                    }
                if(input == ConsoleKey.RightArrow){
                    DockingCrosshair.InertiaX += 1;
                    DockingTarget.moveLeft();
                    }
                AnsiConsole.Cursor.Hide();

                if(DockingTarget.IsOnTarget(DockingCrosshair.CenterX, DockingCrosshair.CenterY)){ // Then check if we're on target
                    break;
                }
            }
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Success!").AllowEmpty());
            return true;
        }
    }
}