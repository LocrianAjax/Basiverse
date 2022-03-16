using System;
using Basiverse;
using System.Collections.Generic;

// TODO
/*
This will contain the Map object that contains all of the Location Objects
*/
namespace Basiverse{
    class Map{ // Map contains the Location nodes
        public List<Location> AllNodes;
    }

    class Location{ // Locations contain names, descriptors and maybe points of interest
        public string Name;
        public string Description;
        public int Type;
        public List<PointofInterest> Interests;
        public List<Location> NearbyNodes;
    }

    class PointofInterest{ // Point of Interest Objects that contain descriptors, names, etc
        public string Name;
        public string Description;
        public int Type;

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