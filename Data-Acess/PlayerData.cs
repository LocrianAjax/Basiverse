using System;
using System.IO;
using System.Linq;
using Basiverse;

namespace Basiverse{
    class Loader{
        /*
            Data Structure:-
            metadata.data contains:
            Save Name
            Other metadata?

            Then use Binary Data Writer to write player OBJ
            savedata.bin
        */
        private string [] Locations;
        public Loader(){
            Locations = new string[3];
            Locations[0] = Directory.GetCurrentDirectory() + "\\Saves\\save1\\";
            Locations[1] = Directory.GetCurrentDirectory() + "\\Saves\\save2\\";
            Locations[2] = Directory.GetCurrentDirectory() + "\\Saves\\save3\\";
        }
        
        public bool CheckSave(int saveNum){
            if(File.Exists(Locations[saveNum] + "metadata.data") && File.Exists(Locations[0] + "savedata.bin")){ // Check for metadata and save
                return true;
            }
            else{
                return false;
            }
        }

        public string GetName(int saveNum){ // Returns the save name from the metadata file
            string [] lines;
            lines = System.IO.File.ReadAllLines(Locations[saveNum] + "metadata.data");
            return lines[0];
        }

        public Player LoadSave(int saveNum){ // Returns a fully loaded player obj
            Player temp = BinarySerialization.ReadFromBinaryFile<Player>(Locations[0] + "savedata.bin");
            return temp;
        }
    
    }

    class Saver{
    /*
        Data Structure:-
        Save Name
        Other metadata?
        Then use Binary Data Writer to write player OBJ      
    */

        private string [] Locations;

        public Saver(){
            Locations = new string[3];
            Locations[0] = Directory.GetCurrentDirectory() + "\\Saves\\save1\\";
            Locations[1] = Directory.GetCurrentDirectory() + "\\Saves\\save2\\";
            Locations[2] = Directory.GetCurrentDirectory() + "\\Saves\\save3\\";
        }

        public int SaveData(int index, string saveName, Player outPlayer){
            string outLocation = Locations[index] + "metadata.data";
            using (StreamWriter sw = new StreamWriter(outLocation)){
                sw.WriteLine(saveName);
            }
            string binLocation = Locations[index] + "savedata.bin";
            BinarySerialization.WriteToBinaryFile<Player>(binLocation, outPlayer);
            return 1;
        }
    }
}