using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse{

    class Starfield{

        private List<Coords> starLocs = new List<Coords>();

        private struct Coords{
            public int x;
            public int y;
            public char star;
        }

        public void Draw(){
            foreach(Coords loc in starLocs){
                AnsiConsole.Cursor.SetPosition(loc.x, loc.y);
                AnsiConsole.Write(loc.star);
            }
        }

        public void Generate(){
            char[] opts = {'.', '\'','`'};
            var rand = new Random();
            int stars = rand.Next(100, 201); 
            for(int i = 0; i < stars; i++){
                int opt = rand.Next(0,3);
                Coords temp = new Coords();
                temp.x = rand.Next(0, Console.WindowWidth);
                temp.y = rand.Next(0, Console.WindowHeight);
                temp.star = opts[opt];
                starLocs.Add(temp);
            }
        }
    }

}