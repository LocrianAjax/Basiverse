using System;
using Basiverse;
using System.Collections.Generic;

// TODO Generate a random map based on data
namespace Basiverse{
    class MapGen{

        public Map Generate(){
            Map outMap = new Map();
            return outMap; // Temp
        }

        private List<PointofInterest> GetPointofInterests(){
            List<PointofInterest> outPoi = new List<PointofInterest>();
            string [] lines;
            lines = System.IO.File.ReadAllLines("Map\\poi.data");
            // TODO parse data in lines and add to list
            return outPoi;
        }

        private List<Location> GetLocations(){
            List<Location> outLocation = new List<Location>();
            string [] lines;
            lines = System.IO.File.ReadAllLines("Map\\location.data");
            // TODO parse data in lines and add to list
            return outLocation;
        }
    }
}