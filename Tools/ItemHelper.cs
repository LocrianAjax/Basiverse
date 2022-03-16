using System;
using Basiverse;
using System.Collections.Generic;
using System.IO;

// TODO
/*
This is a tool to create the .bin files for all of the items in the game. Additional Debug functionality
*/
namespace Basiverse
{
    class ItemHelper
    {
        public void CreateObjects()
        {
            // Hull
            Console.WriteLine("Starting Hull Object Creation");
            List<Hull> Hulls = new List<Hull>();
            string [] hulllines;
            hulllines = System.IO.File.ReadAllLines("Data\\hull.data");
            foreach(string line in hulllines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Hulls.Add(new  Hull(){Name = subs[0], HullMax = Int32.Parse(subs[1]), HeatMax = Int32.Parse(subs[2])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Hull>>("Data\\hull.bin", Hulls);

            // Armor
            Console.WriteLine("Starting Armor Object Creation");
            List<Armor> Armors = new List<Armor>();
            string [] armorlines;
            armorlines = System.IO.File.ReadAllLines("Data\\armor.data");
            foreach(string line in armorlines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Armors.Add(new  Armor(){Name = subs[0], ArmorValue = Int32.Parse(subs[1]), Cost = Int32.Parse(subs[2])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Armor>>("Data\\armor.bin", Armors);

            // Heatsink
            Console.WriteLine("Starting Heatsink Object Creation");
            List<Heatsink> Heatsinks = new List<Heatsink>();
            string [] heatsinklines;
            heatsinklines = System.IO.File.ReadAllLines("Data\\heatsink.data");
            foreach(string line in heatsinklines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Heatsinks.Add(new  Heatsink(){Name = subs[0], PassiveVal = Int32.Parse(subs[1]), ActiveVal = Int32.Parse(subs[2]), Cost = Int32.Parse(subs[3])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Heatsink>>("Data\\heatsink.bin", Heatsinks);

            // Shield
            Console.WriteLine("Starting Shield Object Creation");
            List<Shield> Shields = new List<Shield>();
            string [] shieldlines;
            shieldlines = System.IO.File.ReadAllLines("Data\\shield.data");
            foreach(string line in shieldlines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Shields.Add(new  Shield(){Name = subs[0], ShieldMax = Int32.Parse(subs[1]), Cost = Int32.Parse(subs[2])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Shield>>("Data\\shield.bin", Shields);

            // Laser
            Console.WriteLine("Starting Laser Object Creation");
            List<Laser> Lasers = new List<Laser>();
            string [] laserlines;
            laserlines = System.IO.File.ReadAllLines("Data\\laser.data");
            foreach(string line in laserlines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Lasers.Add(new  Laser(){Name = subs[0], Damage = Int32.Parse(subs[1]), Heat = Int32.Parse(subs[2]), Cost = Int32.Parse(subs[3])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Laser>>("Data\\laser.bin", Lasers);

            // Missile
            Console.WriteLine("Starting Missile Object Creation");
            List<Missile> Missiles = new List<Missile>();
            string [] missilelines;
            missilelines = System.IO.File.ReadAllLines("Data\\missile.data");
            foreach(string line in missilelines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Missiles.Add(new  Missile(){Name = subs[0], Damage = Int32.Parse(subs[1]), HitChance = Double.Parse(subs[2]), Cost = Int32.Parse(subs[3])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Missile>>("Data\\missile.bin", Missiles);

            // Engine
            Console.WriteLine("Starting Engine Object Creation");
            List<Engine> Engines = new List<Engine>();
            string [] enginelines;
            enginelines = System.IO.File.ReadAllLines("Data\\engine.data");
            foreach(string line in enginelines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Engines.Add(new  Engine(){Name = subs[0], FleeChance = Double.Parse(subs[1]), Cost = Int32.Parse(subs[2])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Engine>>("Data\\engine.bin", Engines);

            // CargoHold
            Console.WriteLine("Starting CargoHold Object Creation");
            List<CargoHold> CargoHolds = new List<CargoHold>();
            string [] cargoholdlines;
            cargoholdlines = System.IO.File.ReadAllLines("Data\\cargohold.data");
            foreach(string line in cargoholdlines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    CargoHolds.Add(new  CargoHold(){Name = subs[0], MaxSize = Int32.Parse(subs[1])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<CargoHold>>("Data\\cargohold.bin", CargoHolds);

            // Cargo
            Console.WriteLine("Starting Cargo Object Creation");
            List<Cargo> Cargos = new List<Cargo>();
            string [] cargolines;
            cargolines = System.IO.File.ReadAllLines("Data\\cargo.data");
            foreach(string line in cargolines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    Cargos.Add(new  Cargo(){Name = subs[0], Size = Int32.Parse(subs[1]), Cost = Int32.Parse(subs[2])});
                }
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Cargo>>("Data\\cargo.bin", Cargos);
            Console.WriteLine("\nComplete, vertifying files exist");
            
            string[] Locations = new string[] {"hull", "armor", "shield", "heatsink", "laser", "missile", "cargohold", "cargo"};
            foreach(string location in Locations){
                if(File.Exists($"Data\\{location}.bin")){
                    Console.WriteLine($"{location}.bin vertified");
                }
                else{
                    Console.WriteLine($"Error! {location}.bin not vertified!");
                }
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    
        public void LoadObjTest(){
            Console.WriteLine("Loading .bin data from files testing\n");
            List<Hull> Hulls = BinarySerialization.ReadFromBinaryFile<List<Hull>>("Data\\hull.bin");
            Console.WriteLine("Hull Data");
            foreach(Hull temp in Hulls){
                Console.WriteLine($"{temp.Name} {temp.HullMax} {temp.HeatMax}\n");
            }

            List<Armor> Armors = BinarySerialization.ReadFromBinaryFile<List<Armor>>("Data\\armor.bin");
            Console.WriteLine("Armor Data");
            foreach(Armor temp in Armors){
                Console.WriteLine($"{temp.Name} {temp.ArmorValue} {temp.Cost}\n");
            }

            List<Heatsink> Heatsinks = BinarySerialization.ReadFromBinaryFile<List<Heatsink>>("Data\\heatsink.bin");
            Console.WriteLine("Heatsink Data");
            foreach(Heatsink temp in Heatsinks){
                Console.WriteLine($"{temp.Name} {temp.ActiveVal} {temp.PassiveVal} {temp.Cost}\n");
            }

            List<Shield> Shields = BinarySerialization.ReadFromBinaryFile<List<Shield>>("Data\\shield.bin");
            Console.WriteLine("Shield Data");
            foreach(Shield temp in Shields){
                Console.WriteLine($"{temp.Name} {temp.ShieldMax} {temp.Cost}\n");
            }

            List<Laser> Lasers = BinarySerialization.ReadFromBinaryFile<List<Laser>>("Data\\laser.bin");
            Console.WriteLine("Laser Data");
            foreach(Laser temp in Lasers){
                Console.WriteLine($"{temp.Name} {temp.Damage} {temp.Heat} {temp.Cost}\n");
            }

            List<Missile> Missiles = BinarySerialization.ReadFromBinaryFile<List<Missile>>("Data\\missile.bin");
            Console.WriteLine("Missile Data");
            foreach(Missile temp in Missiles){
                Console.WriteLine($"{temp.Name} {temp.Damage} {temp.HitChance} {temp.Cost}\n");
            }

            List<Engine> Engines = BinarySerialization.ReadFromBinaryFile<List<Engine>>("Data\\engine.bin");
            Console.WriteLine("Engine Data");
            foreach(Engine temp in Engines){
                Console.WriteLine($"{temp.Name} {temp.FleeChance} {temp.Cost}\n");
            }

            List<CargoHold> CargoHolds = BinarySerialization.ReadFromBinaryFile<List<CargoHold>>("Data\\cargohold.bin");
            Console.WriteLine("Cargohold Data");
            foreach(CargoHold temp in CargoHolds){
                Console.WriteLine($"{temp.Name} {temp.MaxSize}\n");
            }
            
            List<Cargo> Cargos = BinarySerialization.ReadFromBinaryFile<List<Cargo>>("Data\\cargo.bin");
            Console.WriteLine("Cargo Data");
            foreach(Cargo temp in Cargos){
                Console.WriteLine($"{temp.Name} {temp.Size} {temp.Cost}\n");
            }
            
            Console.Write("\nLoad complete. Press any key to continue....");
            Console.ReadKey();
        }
    }
}