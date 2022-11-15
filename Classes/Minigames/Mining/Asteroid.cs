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
        }

        public Asteroid(int inSize){ // Specified grid size
            MaxHealth = 100;
            CurrHealth = MaxHealth;
            Size = inSize;
        }

        public void DrawAll(){
            foreach(Tile tile in Tiles){
                tile.Draw();
            }
        }

        public void DrawAt(int index){
            Tiles[index].Draw();
        }

        public void GenerateTiles(){
            var rand = new Random();
            
            for(int i = 0; i <= (Size * Size - 1); i++){
                int Type = rand.Next(5,9);
                Tile temp = new Tile(i, Type, Size);
                Tiles.Add(temp);
            }
        }
    }
}