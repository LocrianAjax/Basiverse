using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Asteroid
    {
        public int Type { get; set; }
        public List<Cargo> Loot { get; set; }
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }
    }
}