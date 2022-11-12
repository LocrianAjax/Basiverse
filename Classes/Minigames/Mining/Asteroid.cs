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
        public int Size {get; set;}

        public Asteroid(){ // Default
            MaxHealth = 100;
            CurrHealth = MaxHealth;
            Size = 6;
            for(int i = 0; i <= 35; i++){
                Tile temp = new Tile(i, Size);
                Tiles.Add(temp);
            }
        }

        public Asteroid(int inSize){ // Specified grid size
            MaxHealth = 100;
            CurrHealth = MaxHealth;
            Size = inSize;
            for(int i = 0; i <= 35; i++){
                Tile temp = new Tile(i, Size);
                Tiles.Add(temp);
            }
        }

        public void Draw(){
            foreach(Tile temp in Tiles){
                temp.Draw();
            }
        }

        public void DrawAt(int index){
            Tiles[index].Draw();
        }
    }
}