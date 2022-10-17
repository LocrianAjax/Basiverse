using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Target{ // Contains the upper left coords of the Docking Target and the method to draw it, clear it
        public int X { get; set;}
        public int Y { get; set;}

        private int InnerX { get; set;}
        private int InnerY { get; set;}

        public Target(){
            var rand = new Random();
            int inX = rand.Next(0, Console.WindowWidth + 1);
            int inY = rand.Next(0, Console.WindowHeight + 1);
            if(IsValid(inX, inY)){
                X = inX;
                InnerX = X + 6;
                Y = inY;
                InnerY = Y + 1;
            }
            else{ // Default to top left if position is invalid although it should never be
                X = 0;
                Y = 0;
            }
        }

        public Target(int inX, int inY){
            if(IsValid(inX, inY)){
                X = inX;
                InnerX = X + 6;
                Y = inY;
                InnerY = Y + 1;
            }
            else{ // Default to top left if position is invalid
                X = 0;
                Y = 0;
            }
        }

        public void Draw(){
            /*
                .------------------.
                |     .------.     |
                |     |      |     |
                |     `------'     |
                `------------------'
            */
            //  We draw from the top left of the obj
            //  NOTE: Console coords have 0,0 top left. X increases L -> R and Y increases Top -> Bottom
            Console.SetCursorPosition(X, Y);
            Console.Write(".------------------.");
            Console.SetCursorPosition(X, Y + 1);
            Console.Write("|     .------.     |");
            Console.SetCursorPosition(X, Y + 2);
            Console.Write("|     |      |     |");
            Console.SetCursorPosition(X, Y + 3);
            Console.Write("|     `------'     |");
            Console.SetCursorPosition(X, Y + 4);
            Console.Write("`------------------'");
        }

        public void Clear(){
            Console.SetCursorPosition(X, Y);
            Console.Write("                    ");
            Console.SetCursorPosition(X, Y + 1);
            Console.Write("                    ");
            Console.SetCursorPosition(X, Y + 2);
            Console.Write("                    ");
            Console.SetCursorPosition(X, Y + 3);
            Console.Write("                    ");
            Console.SetCursorPosition(X, Y + 4);
            Console.Write("                    ");
        }

        public bool IsValid(int inX, int inY){ // Takes in the position and checks to see if it fits on the screen
            if(inX < 0 || inY < 0){ // Check we don't go negative
                return false;
            }
            if(((inX + 20) > Console.WindowWidth) || ((inY + 4) > Console.WindowHeight)){ // Check X and Y
                return false;
            }
            else{
                return true; // If all of those work, return valid
            }
        }

        public void Drift(){ // Causes a random drift in both axies
            var rand = new Random();
            int driftX = rand.Next(-3, 3);
            int driftY = rand.Next(-3,3);
            
            if(IsValid(X + driftX, Y + driftY)){ // Check for both, then just X or Y
                X = X + driftX;
                InnerX = X + 6;
                Y = Y + driftY;
                InnerY = Y + 1;
            }
            else if(IsValid(X + driftX, Y)){
                X = X + driftX;
                InnerX = X + 6;
            }
            else if(IsValid(X, Y + driftY)){
                Y = Y + driftY;
                InnerY = Y + 1;
            }
        }

        public bool IsOnTarget(int TestX, int TestY){
            if(((TestX > InnerX) && (TestX < InnerX + 7)) && ((TestY > InnerY) && (TestY < InnerY + 3))){ // Checking to see if we're in the little space in the center
                return true;
            }
            else{
                return false;
            }
        }
    }
}