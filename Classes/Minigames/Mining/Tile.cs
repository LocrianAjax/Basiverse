using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Tile{

        // Empty Tile
        // ┌───────┐
        // │       │
        // │       │
        // └───────┘

        // Tile with loot
        // ┌───────┐
        // │  ▽ ▽ │
        // │ ▽ ▽  │
        // └───────┘

        // Tile about to be completed
        // ┌───────┐
        // │▚▚▚▚│
        // │▚▚▚▚│
        // └───────┘

        // Tile with rocks that will slowly add ▚ two spaces at a time
        // ┌───────┐
        // │┌─┐┌─┐ │
        // │└─┘└─┘ │
        // └───────┘

        // Un-Mineable
        // ┌───────┐
        // │╳╳╳╳╳╳╳│
        // │╳╳╳╳╳╳╳│
        // └───────┘

        public int Width { get; set; } // Width in chars
        public int Height { get; set; } // Height in lines
        public int Location { get; set; } // From 0 - 24, 0 top left 24 bottom right
        public State CurrState { get; set; } 
        public enum State{
            Loot,
            Empty,
            InProg,
            Rocks,
            Impossible
        }
    }
}