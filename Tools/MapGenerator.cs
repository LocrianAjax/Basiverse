using System;
using Basiverse;
using System.Collections.Generic;

// TODO: Fix mapgen to remove duplicates

// Generates a random map based on locations and POIS from data files
namespace Basiverse{
    class MapGen{

        public Map Generate(bool debug){
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
            if(debug){Console.WriteLine("Getting Locations and POIs");}
            List<Location> allLocations = GetLocations();
            List<PointofInterest> allPoi = GetPointofInterests();
            
            // Get the size of locations and pois
            int locSize = allLocations.Count;
            int poiSize = allPoi.Count;
            if(debug){Console.WriteLine($"Locsize - {locSize} PoiSize - {poiSize}");}
            foreach(Location loc in allLocations){ // Fill out locations
                // Add the POIs
                if(loc.Type == 1){ // Type 1 gets basic POI only
                    if(debug){Console.WriteLine("Type 1");}
                    int numPOI = rand.Next(1, 5); // Generates a number between 0-4
                    if(debug){Console.WriteLine($"Adding {numPOI} POIs");}
                    for(int i = 0; i < numPOI; i++){
                        int randPOI = rand.Next(0, poiSize);
                        if(debug){Console.WriteLine($"Getting POI at position {randPOI}");}
                        while((allPoi[randPOI].Type != 1) && (!loc.Interests.Contains(allPoi[randPOI]))){ // Get a random POI of type 1
                            if(debug){Console.WriteLine("POI wasn't type 1, or already exists retrying");}
                            randPOI = rand.Next(0, poiSize);
                            if(debug){Console.WriteLine($"Getting POI at position {randPOI}");}
                        }
                        loc.Interests.Add(allPoi[randPOI]); // Add it to the list
                        if(debug){Console.WriteLine("Added to the interests list");}
                    }
                }
                else if(loc.Type == 2){ // Type 2 gets just stations
                    if(debug){Console.WriteLine("Type 2");}
                    int numPOI = rand.Next(1, 5); // Generates a number between 0-4
                    if(debug){Console.WriteLine($"Adding {numPOI} POIs");}
                    for(int i = 0; i < numPOI; i++){
                        int randPOI = rand.Next(0, poiSize); // Grab a POI
                        if(debug){Console.WriteLine($"Getting POI at position {randPOI}");}
                        while(allPoi[randPOI].Type != 2){ // If it's not a station we try again
                            if(debug){Console.WriteLine("POI wasn't type 2, retrying");}
                            randPOI = rand.Next(0, poiSize);
                            if(debug){Console.WriteLine($"Getting POI at position {randPOI}");}
                        }
                        if(debug){Console.WriteLine("Adding a station and removing it from the POI list");}
                        loc.Interests.Add(allPoi[randPOI]); // Add it to the list
                        // Then instate the station and add it to the Stations list
                        Station newStation = new Station(allPoi[randPOI].Name, allPoi[randPOI].StationType, allPoi[randPOI].Description);
                        loc.Stations.Add(newStation);
                        if(newStation.Name != "Derelect"){
                            allPoi.RemoveAt(randPOI);
                        }
                        poiSize = allPoi.Count;
                    }
                }
                else if(loc.Type == 3){ // Type 3 gets both
                    if(debug){Console.WriteLine("Type 3");}
                    int numPOI = rand.Next(1, 3); // Generates a number between 1-3 because we're adding a station later
                    if(debug){Console.WriteLine($"Adding {numPOI} POIs");}
                    for(int i = 0; i < numPOI; i++){
                        int randPOI = rand.Next(0, poiSize); // Grab a POI
                        if(debug){Console.WriteLine($"Getting POI at position {randPOI}");}
                        loc.Interests.Add(allPoi[randPOI]); // Add it to the list
                        if(allPoi[randPOI].Type == 2){ // If it's a station, remove it from the list and decrease the size to make sure there are no repeats
                            // Then instate the station and add it to the Stations list
                            Station newStation = new Station(allPoi[randPOI].Name, allPoi[randPOI].StationType, allPoi[randPOI].Description);
                            loc.Stations.Add(newStation);
                            if(debug){Console.WriteLine("Adding a station and removing it from the POI list");}
                            if(newStation.Name != "Derelect"){
                                allPoi.RemoveAt(randPOI);
                            }
                            poiSize = allPoi.Count;
                        }
                    }
                    // Make sure we add at least 1 station
                    int rand2 = rand.Next(0, poiSize); // Grab a POI
                    if(debug){Console.WriteLine($"Getting POI at position {rand2}");}
                    while(allPoi[rand2].Type != 2){ // If it's not a station we try again
                        if(debug){Console.WriteLine("POI wasn't type 2, retrying");}
                        rand2 = rand.Next(0, poiSize);
                        if(debug){Console.WriteLine($"Getting POI at position {rand2}");}
                    }
                    if(debug){Console.WriteLine("Adding a station and removing it from the POI list");}
                    loc.Interests.Add(allPoi[rand2]); // Add it to the list
                    // Then instate the station and add it to the Stations list
                    Station newStation2 = new Station(allPoi[rand2].Name, allPoi[rand2].StationType, allPoi[rand2].Description);
                    loc.Stations.Add(newStation2);
                    if(allPoi[rand2].Name != "Derelect"){ // Keep derelict around to fill in gaps
                        allPoi.RemoveAt(rand2);
                    }
                    poiSize = allPoi.Count;
                    
                }
                else{ // Case 0, null it out
                    if(debug){Console.WriteLine("Type 0: Moving on");}
                    loc.Interests = null;
                }
                
                // Add the links to other nodes
                int numLoc = rand.Next(1, 5); // Generates a number between 0-4
                if(debug){Console.WriteLine($"Adding {numLoc} links");}
                for(int i = 0; i < numLoc; i++){
                    int randLoc = rand.Next(0, locSize);
                    if(debug){Console.WriteLine($"Checking if Location at index {randLoc} is in use or ourself");}
                    if((!loc.NearbyNodes.Contains(allLocations[randLoc])) && (allLocations[randLoc] != loc)){ // Check if it already contains that location and skip if it does
                        if(debug){Console.WriteLine($"Linking the two together");}
                        // Link them both to each other 
                        loc.NearbyNodes.Add(allLocations[randLoc]);
                        allLocations[randLoc].NearbyNodes.Add(loc);
                    }
                    else{
                        Console.WriteLine("Skipping");
                    }
                }
                if(debug){Console.WriteLine("Adding the location to the list of nodes");}
                outMap.AllNodes.Add(loc); // Then add the loc to the map
            }
            if(debug){Console.WriteLine("Map gen complete");
            Console.WriteLine("Press any Key to continue......");
            Console.ReadKey();}
            return outMap;
        }

        public void CheckBin(){
            Map dMap = BinarySerialization.ReadFromBinaryFile<Map>("Map//map.bin");
            dMap.dump();
            Console.WriteLine("Press any Key to continue......");
            Console.ReadKey();
        }
    
        private List<PointofInterest> GetPointofInterests(){
            List<PointofInterest> outPoi = new List<PointofInterest>();
            string [] lines;
            lines = System.IO.File.ReadAllLines("Map//poi.data");
            foreach(string line in lines){
                if(line.Contains("//")){
                    continue; // Ignore all lines with // in them
                }
                else{
                    string[] subs = line.Split('|');
                    if(Int32.Parse(subs[2]) == 2){ // If we have a station type, set it
                        outPoi.Add(new PointofInterest(){Name = subs[0], Description = subs[1], Type = Int32.Parse(subs[2]), StationType = subs[3]});
                    }
                    else{
                        outPoi.Add(new PointofInterest(){Name = subs[0], Description = subs[1], Type = Int32.Parse(subs[2])});
                    }
                }
            }
            return outPoi;
        }

        private List<Location> GetLocations(){
            List<Location> outLocation = new List<Location>();
            string [] lines;
            lines = System.IO.File.ReadAllLines("Map//location.data");
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