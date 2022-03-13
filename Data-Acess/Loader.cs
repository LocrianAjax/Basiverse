using System;
using System.IO;
using System.Linq;
using Basiverse;

namespace Basiverse
{
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
            Locations[0] = Directory.GetCurrentDirectory() + "\\save1\\";
            Locations[1] = Directory.GetCurrentDirectory() + "\\save2\\";
            Locations[2] = Directory.GetCurrentDirectory() + "\\save3\\";
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
}