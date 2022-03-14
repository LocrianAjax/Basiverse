using System;
using Basiverse;
using System.Collections.Generic;

// TODO
/*
This is a tool to create the .bin files for all of the items in the game. Only needs to be run when items are added
*/
namespace Basiverse{

    class ItemWriterHelper{
        public void CreateObjects(){
            // Hull
            string [] HullName =  new string []{"Basic Pressure Hull", "Reinforced Pressure Hull", "Heat Adapted Hull", "Destroyer Hull", "Cruiser Hull", "Battlecruiser Hull"};
            int [] HullMax = new int []{50, 75, 75, 100, 150, 200};
            int [] HullHeatMax = new int []{25, 25, 50, 50, 75, 100};
            // Armor
            string [] ArmorName = new string []{"None", "Thin Steel Plating", "Basic Armor Plates", "Titanium Plating", "Combined Materials", "Reactive Armor"};
            int [] ArmorValue = new int []{0, 10, 25, 50, 75, 150};
            int [] ArmorCost = new int []{0, 50, 100, 150, 175, 200};
            // Heatsink
            string [] HeatsinkName = new string []{"None", "Passive Radiator", "Basic Heatsink", "Advanced Heatsink", "Heat Dissipation System", "Advanced Heat Siphon"};
            int [] HeatsinkPassive = new int []{0, 10, 30, 50, 75, 125};
            int [] HeatsinkActive = new int []{0, 0, 50, 75, 100, 150};
            int [] HeatsinkCost = new int []{0, 50, 100, 150, 175, 200};
            // Shield
            string [] ShieldName = new string []{"Basicorps Asteroid-B-Gone", "Genericor Las-Deflect", "ShieldTech Ray Shield Model Alpha", "ShieldTech Ray Shield Model Omega", "Milspec Deflector Shields", "'Fortress' Advanced Deflectors"};
            int [] ShieldMax = new int []{30, 60, 120, 180, 240, 300};
            int [] ShieldCost = new int []{0, 70, 120, 170, 195, 250};
            // Laser
            string [] LaserName = new string []{};
            int [] LaserDamage = new int []{};
            int [] LaserHeat = new int []{};
            int [] LaserCost = new int []{};
            // Missile
            string [] MissileName = new string []{};
            int [] MissileDamage = new int []{};
            double [] MissileHit = new double []{};
            int [] MissileCost = new int []{0, 70, 120, 170, 195, 250};
            // Engine
            string [] EngineName = new string []{};
            double [] EngineFlee = new double []{};
            int [] EngineCost = new int []{0, 70, 120, 170, 195, 250};
            // CargoHold
            string [] CargoHoldName = new string []{};
            int [] CargoHoldSize = new int []{};
            // Cargo
            string [] CargoName = new string []{};
            int [] CargoSize = new int []{};
            int [] CargoCost = new int []{0, 70, 120, 170, 195, 250};
        }
    }
}