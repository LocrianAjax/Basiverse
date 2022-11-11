using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Crosshair{ // Contains the upper left coords of the crosshair and the method to draw it, clear it, and validate new position 
        public int X { get; set;}
        public int Y { get; set;}

        public int CenterX { get; set;}
        public int CenterY { get; set;}

        public int InertiaX { get; set;}
        public int InertiaY { get; set;}

        public Crosshair(int inX, int inY){ // Constructor sets pos
            X = inX;
            Y = inY;
            CenterX = X + 3;
            CenterY = Y + 1;
            InertiaX = 0;
            InertiaY = 0;
        }

        public Crosshair(){ // Defaults Sets to center
            int inX = Console.WindowWidth / 2;
            int inY = Console.WindowHeight / 2;
            X = inX - 3;
            Y = inY - 1;
            CenterX = X + 3;
            CenterY = Y + 1;
            InertiaX = 0;
            InertiaY = 0;
        }

        public void Draw(){
            /*
                  │
                ──O──
                  │
            */

            /* 
                We draw from the top left of the obj so we write "  |", then move the cursor back down 1 and left 3
                then "--O--" then finally move 5 left and 1 down before writing the final "  |"

                NOTE: Console coords have 0,0 top left. X increases L -> R and Y increases Top -> Bottom
            */ 
            Console.ForegroundColor = ConsoleColor.Green; // Set Color to green for the crosshair and reset after the draw
            AnsiConsole.Cursor.SetPosition(X + 2,Y);
            AnsiConsole.Write("│");
            AnsiConsole.Cursor.SetPosition(X, Y + 1);
            AnsiConsole.Write("──O──");
            AnsiConsole.Cursor.SetPosition(X + 2,Y + 2);
            AnsiConsole.Write("│");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Clear(){ // Same as draw, but overwrites all with spaces
            AnsiConsole.Cursor.SetPosition(X + 2,Y);
            AnsiConsole.Write(" ");
            AnsiConsole.Cursor.SetPosition(X, Y + 1);
            AnsiConsole.Write("     ");
            AnsiConsole.Cursor.SetPosition(X + 2,Y + 2);
            AnsiConsole.Write(" ");
        }

        public bool IsValid(int inX, int inY){ // Takes in the new position and checks to see if it fits on the screen
            if(inX < 0 || inY < 0){ // Check we don't go negative
                return false;
            }
            if(((inX + 5) > Console.WindowWidth) || ((inY + 2) > Console.WindowHeight)){ // Check X and Y
                return false;
            }
            else{
                return true; // If all of those work, return valid
            }
        }

        public bool moveLeft(){
            if(IsValid(X - 1, Y)){
                Clear();
                X -= 1;
                CenterX -= 1;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveRight(){
            if(IsValid(X + 1, Y)){
                Clear();
                X += 1;
                CenterX += 1;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveUp(){
            if(IsValid(X, Y - 1)){
                Clear();
                Y -= 1;
                CenterY -= 1;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveDown(){
            if(IsValid(X, Y + 1)){
                Clear();
                Y += 1;
                CenterY += 1;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
    }
}