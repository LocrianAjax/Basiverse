using System;
using Basiverse;
using System.Collections.Generic;

// TODO
/*
This will contain the Map object that contains all of the Location Objects
*/
namespace Basiverse{ // ADD CONSTRUCTIORS
    class Map{ // Map contains the Location nodes
        public List<Location> AllNodes = new List<Location>();
    }

    class Location{ // Locations contain names, descriptors and maybe points of interest
        public string Name = "";
        public string Description = "";
        public int Type = 0;
        public List<PointofInterest> Interests = new List<PointofInterest>();
        public List<Location> NearbyNodes = new List<Location>();
    }

    class PointofInterest{ // Point of Interest Objects that contain descriptors, names, etc
        public string Name = "";
        public string Description = "";
        public int Type = 0;

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