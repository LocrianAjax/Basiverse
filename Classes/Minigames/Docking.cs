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


        public Crosshair(int inX, int inY){ // Constructor sets pos
            X = inX;
            Y = inY;
        }

        public Crosshair(){ // Defaults to top left
            X = 0;
            Y = 0;
        }

        public void Draw(){
            /*
                  |
                --O--
                  |
            */

            /* 
                We draw from the top left of the obj so we write "  |", then move the cursor back down 1 and left 3
                then "--O--" then finally move 5 left and 1 down before writing the final "  |"

                NOTE: Console coords have 0,0 top left. X increases L -> R and Y increases Top -> Bottom
            */ 
            Console.SetCursorPosition(X + 2,Y);
            Console.Write("|");
            Console.SetCursorPosition(X, Y + 1);
            Console.Write("--O--");
            Console.SetCursorPosition(X + 2,Y + 2);
            Console.Write("|");
        }

        public void Clear(){ // Same as draw, but overwrites all with spaces
            Console.SetCursorPosition(X + 2,Y);
            Console.Write(" ");
            Console.SetCursorPosition(X, Y + 1);
            Console.Write("     ");
            Console.SetCursorPosition(X + 2,Y + 2);
            Console.Write(" ");
        }

        public bool IsValid(int inX, int inY){ // Takes in the new position and checks to see if it fits on the screen
            if(inX < 0 || inY < 0){ // Check we don't go negative
                return false;
            }
            if(((inX + 5) > Console.WindowWidth) || ((inY + 3) > Console.WindowHeight)){ // Check X and Y
                return false;
            }
            else{
                return true; // If all of those work, return valid
            }
        }
    }
}