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
            int inX = rand.Next(5, Console.WindowWidth);
            int inY = rand.Next(5, Console.WindowHeight);
            if(IsValid(inX, inY)){
                X = inX;
                InnerX = X + 6;
                Y = inY;
                InnerY = Y + 1;
            }
            else{ // Default to top left if position is invalid although it should never be
                X = 5;
                Y = 5;
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
                X = 5;
                Y = 5;
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
            string[] lines = 
            {".------------------.",
             "|     .------.     |", 
             "|     |      |     |", 
             "|     `------'     |",
             "`------------------'"};

            int count = 0;
            foreach (string line in lines)
            {
                AnsiConsole.Cursor.SetPosition(X, Y + count);
                Console.Write(line);
                count++;
            }
        }

        public void Clear(){
            AnsiConsole.Cursor.SetPosition(X, Y);
            AnsiConsole.Write("                     ");
            AnsiConsole.Cursor.SetPosition(X, Y + 1);
            AnsiConsole.Write("                     ");
            AnsiConsole.Cursor.SetPosition(X, Y + 2);
            AnsiConsole.Write("                     ");
            AnsiConsole.Cursor.SetPosition(X, Y + 3);
            AnsiConsole.Write("                     ");
            AnsiConsole.Cursor.SetPosition(X, Y + 4);
            AnsiConsole.Write("                     ");
        }

        public bool IsValid(int inX, int inY){ // Takes in the position and checks to see if it fits on the screen
            if(inX < 0 || inY < 1){ // Check we don't go negative
                return false;
            }
            if(((inX + 20) > Console.WindowWidth) || ((inY + 4) > Console.WindowHeight)){ // Check X and Y
                return false;
            }
            else{
                return true; // If all of those work, return valid
            }
        }

        public void Drift(int InertiaX, int InertiaY){ // Causes a random drift in both axies

            
            if(IsValid(X + InertiaX, Y + InertiaY)){ // Check for both, then just X or Y
                Clear();
                X = X + InertiaX;
                InnerX = X + 6;
                Y = Y + InertiaY;
                InnerY = Y + 1;
                Draw();
            }
            else if(IsValid(X + InertiaX, Y)){
                Clear();
                X = X + InertiaX;
                InnerX = X + 6;
                Draw();
            }
            else if(IsValid(X, Y + InertiaY)){
                Clear();
                Y = Y + InertiaY;
                InnerY = Y + 1;
                Draw();
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

        public bool moveLeft(){
            int movement = -1; // Set up the movement
            if(IsValid(X + movement, Y)){
                Clear();
                X += movement;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveRight(){
            int movement = 1;
            if(IsValid(X + movement, Y)){
                Clear();
                X += movement;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveUp(){
            int movement = -1;
            if(IsValid(X, Y + movement)){
                Clear();
                Y += movement;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
        public bool moveDown(){
            int movement = 1;
            if(IsValid(X, Y + movement)){
                Clear();
                Y += movement;
                Draw();
                return true;
            }
            else{
                return false;
            }
        }
    }
}