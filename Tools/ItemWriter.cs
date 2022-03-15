using System;
using Basiverse;
using System.Collections.Generic;
using System.IO;

// TODO
/*
This is a tool to create the .bin files for all of the items in the game. Only needs to be run when items are added
*/
namespace Basiverse
{
    class ItemWriterHelper
    {
        public void CreateObjects()
        {
            // Hull
            Console.WriteLine("Starting Hull Object Creation");
            string[] HullsName = new string[] { "Basic Pressure Hull", "Reinforced Pressure Hull", "Heat Adapted Hull", "Destroyer Hull", "Cruiser Hull", "Battlecruiser Hull" };
            int[] HullsMax = new int[] { 50, 75, 75, 100, 150, 200 };
            int[] HullsHeatMax = new int[] { 25, 25, 50, 50, 75, 100 };
            List<Hull> Hulls = new List<Hull>();

            for(int i = 0; i < HullsName.Length; i++){
                Hulls.Add(new Hull() {Name = HullsName[i], HullMax = HullsMax[i], HeatMax = HullsHeatMax[i] });
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Hull>>("Data\\hull.bin", Hulls);

            // Armor
            Console.WriteLine("Starting Armor Object Creation");
            string[] ArmorsName = new string[] { "None", "Thin Steel Plating", "Basic Armor Plates", "Titanium Plating", "Combined Materials", "Reactive Armor" };
            int[] ArmorsValue = new int[] { 0, 10, 25, 50, 75, 150 };
            int[] ArmorsCost = new int[] { 0, 50, 100, 150, 175, 200 };
            List<Armor> Armors = new List<Armor>();
            for(int i = 0; i < ArmorsName.Length; i++){
                Armors.Add(new Armor() {Name = ArmorsName[i], ArmorValue = ArmorsValue[i], Cost = ArmorsCost[i] });
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Armor>>("Data\\armor.bin", Armors);

            // Heatsink
            Console.WriteLine("Starting Heatsink Object Creation");
            string[] HeatsinksName = new string[] { "None", "Basicorps therm01", "Basicorps therm10", "Advanced Heatsink", "Heat Dissipation System", "Advanced Heat Siphon" };
            int[] HeatsinksPassive = new int[] { 0, 1, 5, 10, 20, 40 };
            int[] HeatsinksActive = new int[] { 0, 10, 50, 75, 100, 150 };
            int[] HeatsinksCost = new int[] { 0, 50, 100, 150, 175, 200 };
            List<Heatsink> Heatsinks = new List<Heatsink>();
            for(int i = 0; i < HeatsinksName.Length; i++){
                Heatsinks.Add(new Heatsink() { Name = HeatsinksName[i], PassiveVal = HeatsinksPassive[i], ActiveVal = HeatsinksActive[i], Cost = HeatsinksCost[i]});
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Heatsink>>("Data\\heatsink.bin", Heatsinks);

            // Shield
            Console.WriteLine("Starting Shield Object Creation");
            string[] ShieldsName = new string[] { "Basicorps Asteroid-B-Gone", "Genericor Las-Deflect", "ShieldTech Ray Shield Model Alpha", "ShieldTech Ray Shield Model Omega", "Milspec Deflector Shields", "'Fortress' Advanced Deflectors" };
            int[] ShieldsMax = new int[] { 30, 60, 120, 180, 240, 300 };
            int[] ShieldsCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Shield> Shields = new List<Shield>();
            for(int i = 0; i < ShieldsName.Length; i++){
                Shields.Add(new Shield() { Name = ShieldsName[i], ShieldMax = ShieldsMax[i], Cost = ShieldsCost[i]});
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Shield>>("Data\\shield.bin", Shields);

            // Laser
            Console.WriteLine("Starting Laser Object Creation");
            string[] LasersName = new string[] { "Basicorps Presentation Laser", "Basicorps Retina-B-Gone", "Basicorps Flamethrower", "Basicorps Mining Laser", "Military Light Fighter Laser", "Military Destroyer Las-Cannon" };
            int[] LasersDamage = new int[] { 5, 10, 50, 30, 75, 100 };
            int[] LasersHeat = new int[] { 2, 5, 15, 10, 15, 20 };
            int[] LasersCost = new int[] { 0, 50, 75, 100, 120, 150 };
            List<Laser> Lasers = new List<Laser>();
            for(int i = 0; i < LasersName.Length; i++){
                Lasers.Add(new Laser(){ Name = LasersName[i], Damage = LasersDamage[i], Heat = LasersHeat[i], Cost = LasersCost[i] });
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Laser>>("Data\\laser.bin", Lasers);

            // Missile
            Console.WriteLine("Starting Missile Object Creation");
            string[] MissilesName = new string[] { "Basicorps Bottle Rockets", "Basicorps Mortars", "Unguided Mil-Spec Rocket", "Heatseaking Missiles", "Radar Guided Cruise Missiles", "Space ICBM" };
            int[] MissilesDamage = new int[] { 10, 15, 25, 25, 50, 75 };
            double[] MissilesHit = new double[] { .6, .6, .7, .85, .8, .8 };
            int[] MissilesCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Missile> Missiles = new List<Missile>();
            for(int i = 0; i < MissilesName.Length; i++){
                Missiles.Add(new Missile(){ Name = MissilesName[i], Damage = MissilesDamage[i], HitChance = MissilesHit[i], Cost = MissilesCost[i] });
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Missile>>("Data\\missile.bin", Missiles);

            // Engine
            Console.WriteLine("Starting Engine Object Creation");
            string[] EnginesName = new string[] { "Basic Manuvering Thrusters", "Advanced RCS System", "Gyro-Stabilized Thrusters", "Gimbling Thrusters", "Milspec Fighter Thrusters", "Advanced Racing Thrusters" };
            double[] EnginesFlee = new double[] { .3, .4, .5, .6, .7, .8 };
            int[] EnginesCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Engine> Engines = new List<Engine>();
            for(int i = 0; i < EnginesName.Length; i++){
                Engines.Add(new Engine(){ Name = EnginesName[i], FleeChance = EnginesFlee[i], Cost = EnginesCost[i] });
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<Engine>>("Data\\engine.bin", Engines);

            // CargoHold
            Console.WriteLine("Starting CargoHold Object Creation");
            string[] CargoHoldsName = new string[] { "Glovebox", "Personal Locker", "Spare Berth", "Basicorps Cargohold Mini-01", "Basicorps Cargohold Maxi-10", "Basicorps Cargohold Maxi-100" };
            int[] CargoHoldsSize = new int[] { 5, 10, 25, 50, 100, 500, 1000 };
            List<CargoHold> CargoHolds = new List<CargoHold>();
            for(int i = 0; i < CargoHoldsName.Length; i++){
                CargoHolds.Add(new CargoHold(){ Name = CargoHoldsName[i], MaxSize = CargoHoldsSize[i]});
            }
            Console.WriteLine("Object list created, writing to .bin");
            BinarySerialization.WriteToBinaryFile<List<CargoHold>>("Data\\cargohold.bin", CargoHolds);

            // Cargo
            Console.WriteLine("Starting Cargo Object Creation");
            string[] CargosName = new string[] { "Ration Pack", "Hydration Pack", "Luxury Goods", "Artwork", "Minerals", "Space Gems" };
            int[] CargosSize = new int[] { 1, 3, 5, 10, 15, 50 };
            int[] CargosCost = new int[] { 10, 20, 120, 170, 195, 250 };
            List<Cargo> Cargos = new List<Cargo>();
            for(int i = 0; i < CargosName.Length; i++){
                Cargos.Add(new Cargo(){ Name = CargosName[i], Size = CargosSize[i], Cost = CargosCost[i]});
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
    }
}