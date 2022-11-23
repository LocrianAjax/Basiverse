using System;
using Basiverse;
using System.Collections.Generic;

/*
This contains the Map object that contains all of the Location Objects
*/
namespace Basiverse{
    [Serializable]
    class Map{ // Map contains the Location nodes
        public List<Location> AllNodes = new List<Location>();

        public void dump(){
            Console.WriteLine("Dumping Map Data\n");
            foreach(Location node in AllNodes){
                node.dump();
            }
            Console.WriteLine("Press Enter to continue......");
            Console.ReadKey();
        }
    }


    [Serializable]
    class Location{ // Locations contain names, descriptors and maybe points of interest
        public string Name = "";
        public string Description = "";
        public int Type = 0;
        public List<PointofInterest> Interests = new List<PointofInterest>();
        public List<Location> NearbyNodes = new List<Location>();

        public List<Station> Stations = new List<Station>();

        public void dump(){
            Console.WriteLine($"\nLocation Name: {Name} Description: {Description} Type: {Type}");
            Console.WriteLine("Points of Interest:-");
            if(Interests != null){
                foreach(PointofInterest poi in Interests){
                    poi.dump();
                }
            }
            else{
                Console.WriteLine("No POIs at this location\n");
            }
            Console.WriteLine("\nLinked Nodes:-");
            foreach(Location loc in NearbyNodes){
                Console.WriteLine($"{loc.Name}");
            }

        }
    }

    [Serializable]
    class PointofInterest{ // Point of Interest Objects that contain descriptors, names, etc
        public string Name = "";
        public string Description = "";
        public int Type = 0;

        public string StationType; // Just for stations

        public void dump(){
            if(StationType != null){
                Console.WriteLine($"POI Name: {Name} Description: {Description} Type: {Type} StationType: {StationType}\n");
            }
            else{
                Console.WriteLine($"POI Name: {Name} Description: {Description} Type: {Type}\n");
            }
        }

        /* Possible Additions - 
           NPCs
           Merchant Vessels
           Bases
           Mining Colonies
           Warships
           Pirates
           etc.
        */ 

    }
}