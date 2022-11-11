using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Tile{
        string[] BlankingTile = 
        {"         ",
         "         ",
         "         ",
         "         "};

        string[] EmptyTile = 
        {"┌───────┐",
         "│       │",
         "│       │",
         "└───────┘"
         };

        string[] LootATile = 
        {"┌───────┐",
         "│  ▽ ▽ │",
         "│ ▽ ▽  │",
         "└───────┘"
         };

        string[] LootBTile = 
        {"┌───────┐",
         "│ ▽ ▽  │",
         "│  ▽ ▽ │",
         "└───────┘"
         };

        string[] Stage4Tile = 
        {"┌───────┐",
         "│▚▚▚▚│",
         "│▚▚▚▚│",
         "└───────┘"
         };

         string[] Stage3Tile = 
        {"┌───────┐",
         "│▚▚▚  │",
         "│▚▚▚  │",
         "└───────┘"
         };     
            
         string[] Stage2Tile = 
        {"┌───────┐",
         "│▚▚─┐ │",
         "│▚▚─┐ │",
         "└───────┘"
         };      

         string[] Stage1Tile = 
        {"┌───────┐",
         "│▚┐┌─┐ │",
         "│▚┘└─┘ │",
         "└───────┘"
         };     

         string[] BaseTile = 
        {"┌───────┐",
         "│┌─┐┌─┐ │",
         "│└─┘└─┘ │",
         "└───────┘"
         };        
        
        string[] ImpossibleTile = 
        {"┌───────┐",
         "│╳╳╳╳╳╳╳│",
         "│╳╳╳╳╳╳╳│",
         "└───────┘"
         };

        public int Width { get; set; } // Width in chars (9 is default)
        public int Height { get; set; } // Height in lines (4 is default)
        public int Location { get; set; } // 0 is top left and 35 is bottom right on the 6x6 grid
        public State CurrState { get; set; } 
        public bool Loot { get; set; }
        public enum State{
            LootA,
            LootB,
            Empty,
            Stage4,
            Stage3,
            Stage2,
            Stage1,
            Base,
            Impossible
        }

    
        public Tile(int inLoc){ // Default constructor
            Width = 9;
            Height = 4;
            Location = inLoc;
            CurrState = State.Empty;
            Loot = false;
        }
        
        public Tile(int inLoc, State inState){ // Overloaded
            Width = 9;
            Height = 4;
            Location = inLoc;
            CurrState = inState;
            if(CurrState == State.LootA || CurrState == State.LootB){
                Loot = true;
            }
            else{
                Loot = false;
            }
        }

        public Tile(int inLoc, State inState, bool inLoot){ // Overloaded
            Width = 9;
            Height = 4;
            Location = inLoc;
            CurrState = inState;
            Loot = inLoot;
        }

        public void Draw(){
            /* We have to make some assumptions
               1: All tiles are the same size
               2: We're on the 6x6 grid, with 0 being top left, 5 being top right etc
            
                Row 0: 0 - 5
                Row 1: 6 - 11
                Row 2: 12-17
                Row 3: 18 - 23
                Row 4: 24 - 29
                Row 5: 30 - 35
            */
            string[] lines;
            switch(CurrState){
                case State.LootA:
                    lines = LootATile;
                break;
                case State.LootB:
                     lines = LootBTile;
                break;
                case State.Base:
                    lines = BaseTile;
                break;
                case State.Impossible:
                    lines = ImpossibleTile;
                break;
                case State.Stage1:
                    lines = Stage1Tile;
                break;
                case State.Stage2:
                    lines = Stage2Tile;
                break;
                case State.Stage3:
                    lines = Stage3Tile;
                break;
                case State.Stage4:
                    lines = Stage4Tile;
                break;
                case State.Empty:
                    lines = EmptyTile;
                break;
                default:
                    lines = EmptyTile;
                break;
            }
            // First we find the center and align our Top Left (our 0,0)
            int TLX = (Console.WindowWidth / 2) - (3 * Width);
            int TLY = (Console.WindowHeight / 2) - (3 * Height);

            // Then we adjust for our tile's location on Y
            int row = Location / 6;
            int currY = TLY + (row * Height);
            // And on X
            int col = Location % 6;
            int currX = TLX + (col * Width);

            // Then we go to the position and clear the tile
            int count = 0;
            foreach (string Blank in BlankingTile)
            {
                AnsiConsole.Cursor.SetPosition(currX, currY + count);
                Console.Write(Blank);
                count++;
            }
            // Then draw the updated tile
            count = 0;
            foreach (string line in lines)
            {
                AnsiConsole.Cursor.SetPosition(currX, currY + count);
                Console.Write(line);
                count++;
            }
        }
    }
}