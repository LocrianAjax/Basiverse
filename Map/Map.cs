using System;
using Basiverse;
using System.Collections.Generic;

// TODO
/*
This will contain the Map object that contains all of the Location Objects
*/
namespace Basiverse{
    class Map{ // Map contains the Location nodes
        List<Location> AllNodes;
    }

    class Location{ // Locations contain names, descriptors and maybe points of interest
        string Name;
        string Description;
        List<PointofInterest> Interests;
        List<Location> NearbyNodes;
    }

    class PointofInterest{ // Point of Interest Objects that contain descriptors, names, etc
        string Name;
        string Description;

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