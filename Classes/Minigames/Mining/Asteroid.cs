using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Asteroid
    {
        public List<Cargo> Loot { get; set; }
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }

        public Asteroid(){
            MaxHealth = 100;
            CurrHealth = MaxHealth;
        }
    }
}