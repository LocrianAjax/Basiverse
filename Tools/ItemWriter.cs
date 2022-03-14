using System;
using Basiverse;
using System.Collections.Generic;

// TODO
/*
This is a tool to create the .bin files for all of the items in the game. Only needs to be run when items are added
*/
namespace Basiverse
{
    class ItemWriterHelper
    {
        public async void CreateObjects()
        {
            // Hull
            int hullNum = 6;
            string[] HullsName = new string[] { "Basic Pressure Hull", "Reinforced Pressure Hull", "Heat Adapted Hull", "Destroyer Hull", "Cruiser Hull", "Battlecruiser Hull" };
            int[] HullsMax = new int[] { 50, 75, 75, 100, 150, 200 };
            int[] HullsHeatMax = new int[] { 25, 25, 50, 50, 75, 100 };
            List<Hull> Hulls = new List<Hull>();

            for(int i = 0; i < hullNum; i++){
                Hulls.Add(new Hull() {Name = HullsName[i], HullMax = HullsMax[i], HeatMax = HullsHeatMax[i] });
            }
            // Armor
            string[] ArmorName = new string[] { "None", "Thin Steel Plating", "Basic Armor Plates", "Titanium Plating", "Combined Materials", "Reactive Armor" };
            int[] ArmorValue = new int[] { 0, 10, 25, 50, 75, 150 };
            int[] ArmorCost = new int[] { 0, 50, 100, 150, 175, 200 };
            List<Armor> Armors;
            // Heatsink
            string[] HeatsinkName = new string[] { "None", "Basicorps therm01", "Basicorps therm10", "Advanced Heatsink", "Heat Dissipation System", "Advanced Heat Siphon" };
            int[] HeatsinkPassive = new int[] { 0, 1, 5, 10, 20, 40 };
            int[] HeatsinkActive = new int[] { 0, 10, 50, 75, 100, 150 };
            int[] HeatsinkCost = new int[] { 0, 50, 100, 150, 175, 200 };
            List<Heatsink> Heatsinks;
            // Shield
            string[] ShieldName = new string[] { "Basicorps Asteroid-B-Gone", "Genericor Las-Deflect", "ShieldTech Ray Shield Model Alpha", "ShieldTech Ray Shield Model Omega", "Milspec Deflector Shields", "'Fortress' Advanced Deflectors" };
            int[] ShieldMax = new int[] { 30, 60, 120, 180, 240, 300 };
            int[] ShieldCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Shield> Shields;
            // Laser
            string[] LaserName = new string[] { "Basicorps Presentation Laser", "Basicorps Retina-B-Gone", "Basicorps Flamethrower", "Basicorps Mining Laser", "Military Light Fighter Laser", "Military Destroyer Las-Cannon" };
            int[] LaserDamage = new int[] { 5, 10, 50, 30, 75, 100 };
            int[] LaserHeat = new int[] { 2, 5, 15, 10, 15, 20 };
            int[] LaserCost = new int[] { 0, 50, 75, 100, 120, 150 };
            List<Laser> Lasers;
            // Missile
            string[] MissileName = new string[] { "Basicorps Bottle Rockets", "Basicorps Mortars", "Unguided Mil-Spec Rocket", "Heatseaking Missiles", "Radar Guided Cruise Missiles", "Space ICBM" };
            int[] MissileDamage = new int[] { 10, 15, 25, 25, 50, 75 };
            double[] MissileHit = new double[] { .6, .6, .7, .85, .8, .8 };
            int[] MissileCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Missile> Missiles;
            // Engine
            string[] EngineName = new string[] { "Basic Manuvering Thrusters", "Advanced RCS System", "Gyro-Stabilized Thrusters", "Gimbling Thrusters", "Milspec Fighter Thrusters", "Advanced Racing Thrusters" };
            double[] EngineFlee = new double[] { .3, .4, .5, .6, .7, .8 };
            int[] EngineCost = new int[] { 0, 70, 120, 170, 195, 250 };
            List<Engine> Engines;
            // CargoHold
            string[] CargoHoldName = new string[] { "Glovebox", "Personal Locker", "Spare Berth", "Basicorps Cargohold Mini-01", "Basicorps Cargohold Maxi-10", "Basicorps Cargohold Maxi-100" };
            int[] CargoHoldSize = new int[] { 5, 10, 25, 50, 100, 500, 1000 };
            List<CargoHold> CargoHolds;
            // Cargo
            string[] CargoName = new string[] { "Ration Pack", "Hydration Pack", "Luxury Goods", "Artwork", "Minerals", "Space Gems" };
            int[] CargoSize = new int[] { 1, 3, 5, 10, 15, 50 };
            int[] CargoCost = new int[] { 10, 20, 120, 170, 195, 250 };
            List<Cargo> Cargos;
        }
    }
}