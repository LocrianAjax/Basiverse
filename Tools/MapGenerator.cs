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
            foreach(string line in lines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    outPoi.Add(new PointofInterest(){Name = subs[0], Description = subs[1], Type = Int32.Parse(subs[2])});
                }
            }
            return outPoi;
        }

        private List<Location> GetLocations(){
            List<Location> outLocation = new List<Location>();
            string [] lines;
            lines = System.IO.File.ReadAllLines("Map\\location.data");
            foreach(string line in lines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    outLocation.Add(new Location(){Name = subs[0], Description = subs[1], Type = Int32.Parse(subs[2])});
                }
            }
            return outLocation;
        }
    }
}