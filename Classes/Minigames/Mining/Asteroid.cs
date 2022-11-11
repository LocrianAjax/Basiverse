using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{
    class Asteroid
    {
        public List<Cargo> Loot = new List<Cargo>();
        public List<Tile> Tiles = new List<Tile>();
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }

        public Asteroid(){
            MaxHealth = 100;
            CurrHealth = MaxHealth;
            for(int i = 0; i <= 35; i++){
                Tile temp = new Tile(i);
                Tiles.Add(temp);
            }
        }

        public void Draw(){
            foreach(Tile temp in Tiles){
                temp.Draw();
            }
        }
    }
}