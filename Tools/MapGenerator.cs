using System;
using Basiverse;
using System.Collections.Generic;

// TODO Generate a random map based on data
namespace Basiverse{
    class MapGen{

        public Map Generate(){
            Map outMap = new Map();
            /* 
                To add POIs - 
                Check which kind of location it is then pick a number of POIs 1-4 if needed.
                Grab a random one, and if it's a station remove it from the list and make sure we use it only once.

                To link locations link to each other - 
                Generate a selection of random numbers from 0 to the size of the location array.
                For each location in the array, generate a number 0 to 4.
                If 0, move on. If 1 select one of those numbers at random and link to it, 2 do it twice, etc.
            */
            var rand = new Random(); // Instantiate random number generator using system-supplied value as seed.

            // Get all the location data and poi data
            Console.WriteLine("Getting Locations and POIs");
            List<Location> allLocations = GetLocations();
            List<PointofInterest> allPoi = GetPointofInterests();
            
            // Get the size of locations and pois
            int locSize = allLocations.Count;
            int poiSize = allPoi.Count;
            Console.WriteLine($"Locsize - {locSize} PoiSize - {poiSize}");
            foreach(Location loc in allLocations){ // Fill out locations
                // Add the POIs
                if(loc.Type == 1){ // Type 1 gets basic POI only
                    Console.WriteLine("Type 1");
                    int numPOI = rand.Next(1, 5); // Generates a number between 0-4
                    Console.WriteLine($"Adding {numPOI} POIs");
                    for(int i = 0; i < numPOI; i++){
                        int randPOI = rand.Next(0, poiSize);
                        Console.WriteLine($"Getting POI at position {randPOI}");
                        while(randPOI != 1){ // Get a random POI of type 1
                            Console.WriteLine("POI wasn't type 1, retrying");
                            randPOI = rand.Next(0, poiSize);
                            Console.WriteLine($"Getting POI at position {randPOI}");
                        }
                        loc.Interests.Add(allPoi[randPOI]); // Add it to the list
                        Console.WriteLine("Added to the interests list");
                    }
                }
                else if(loc.Type == 2){ // Type 2 gets both stations and basics
                    Console.WriteLine("Type 2");
                    int numPOI = rand.Next(1, 5); // Generates a number between 0-4
                    Console.WriteLine($"Adding {numPOI} POIs");
                    for(int i = 0; i < numPOI; i++){
                        int randPOI = rand.Next(0, poiSize); // Grab a POI
                        Console.WriteLine($"Getting POI at position {randPOI}");
                        loc.Interests.Add(allPoi[randPOI]); // Add it to the list
                        if(allPoi[randPOI].Type == 2){ // If it's a station, remove it from the list and decrease the size to make sure there are no repeats
                            Console.WriteLine("Adding a station and removing it from the POI list");
                            allPoi.RemoveAt(randPOI);
                            poiSize = allPoi.Count;
                        }
                    }
                }
                else{ // Case 0, null it out
                    Console.WriteLine("Type 0: Moving on");
                    loc.Interests = null;
                }

                // Add the links to other nodes
                int numLoc = rand.Next(1, 5); // Generates a number between 0-4
                Console.WriteLine($"Adding {numLoc} links");
                for(int i = 0; i < numLoc; i++){
                    int randLoc = rand.Next(0, locSize);
                    Console.WriteLine($"Checking if Location at index {randLoc}");
                    if ((loc.NearbyNodes == null)){
                        Console.WriteLine($"Nearby is null, linking the two together");
                        // Link them both to each other 
                        loc.NearbyNodes.Add(allLocations[randLoc]);
                        allLocations[randLoc].NearbyNodes.Add(loc);
                    }
                    else if(!(loc.NearbyNodes.Contains(allLocations[randLoc]))){ // Check if it already contains that location and skip if it does
                        Console.WriteLine($"Linking the two together");
                        // Link them both to each other 
                        loc.NearbyNodes.Add(allLocations[randLoc]);
                        allLocations[randLoc].NearbyNodes.Add(loc);
                    }
                }
                Console.WriteLine("Adding the location to the list of nodes");
                outMap.AllNodes.Add(loc); // Then add the loc to the map
            }
            Console.WriteLine("Map gen complete");
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