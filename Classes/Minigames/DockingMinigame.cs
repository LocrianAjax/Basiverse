using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{

    class TimedReader {
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static ConsoleKey input;

        static TimedReader() {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(Timedreader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void Timedreader() {
            while (true) {
            getInput.WaitOne();
            input = Console.ReadKey().Key;
            gotInput.Set();
            }
        }

        public static ConsoleKey ReadKey(int timeOutMillisecs = Timeout.Infinite) {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success){
                return input;
            }
            else{
                return ConsoleKey.Backspace;
            }
        }
    }

    class DockingMinigame{
        // TODO: This
        public Target DockingTarget;
        public Crosshair DockingCrosshair;
        public int Fuel = 150;

        public DockingMinigame(){ // Default
            DockingCrosshair = new Crosshair();
            DockingTarget = new Target();
        }

        public DockingMinigame(int TargetX, int TargetY){ // Specified locations
            DockingCrosshair = new Crosshair();
            DockingTarget = new Target(TargetX, TargetY);
        }

        public void DrawDockingAssist(){
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the top row
            AnsiConsole.Write("                                                                                                            ");
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the top row
            AnsiConsole.Markup($"[green]DOCKING ASSIST ONLINE:  REMAINING FUEL {Fuel}[/]");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the top row
            AnsiConsole.Write("                                                                                                            ");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the top row
            AnsiConsole.Markup($"[green]RELATIVE SPEEDS - YAW SPEED: {DockingCrosshair.InertiaX}m/s PITCH SPEED: {DockingCrosshair.InertiaY}m/s[/]");
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
            DrawDockingAssist();
            DockingTarget.Draw();
            AnsiConsole.Cursor.Hide();
            DockingCrosshair.Draw();
            AnsiConsole.Cursor.Hide();
            //  Now that we're set up, start the loop
            while(true){
                DrawDockingAssist();
                DockingTarget.Drift(DockingCrosshair.InertiaX, DockingCrosshair.InertiaY); // Drift with inertia
                DockingCrosshair.Draw();

                ConsoleKey input = TimedReader.ReadKey(250);
                // Once we get the input (if any) we reduce fuel, move and increase the inertia. Regardless of if we move or not we drift according to the inertia
                if(input == ConsoleKey.DownArrow){
                    Fuel--;
                    DockingCrosshair.InertiaY += 1;
                    DockingTarget.moveDown();
                }
                if(input == ConsoleKey.UpArrow){
                    Fuel--;
                    DockingCrosshair.InertiaY -= 1;
                    DockingTarget.moveUp();
                }
                if(input == ConsoleKey.LeftArrow){
                    Fuel--;
                    DockingCrosshair.InertiaX -= 1;
                    DockingTarget.moveRight();
                }
                if(input == ConsoleKey.RightArrow){
                    Fuel--;
                    DockingCrosshair.InertiaX += 1;
                    DockingTarget.moveLeft();
                }
                AnsiConsole.Cursor.Hide();

                if(DockingTarget.IsOnTarget(DockingCrosshair.CenterX, DockingCrosshair.CenterY)){ // Then check if we're on target
                    var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Success!").AllowEmpty());
                    return true;
                }
                if(Fuel <= 0){
                    var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Docking fuel empty!").AllowEmpty());
                    return false;
                }
            }
        }
    }
}