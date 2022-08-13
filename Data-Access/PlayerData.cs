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
        
        public bool CheckSave(){
            if(File.Exists(Directory.GetCurrentDirectory() + "\\Save\\metadata.data") && File.Exists(Directory.GetCurrentDirectory() + "\\Save\\savedata.bin")){ // Check for metadata and save
                return true;
            }
            else{
                return false;
            }
        }

        public string GetName(){ // Returns the save name from the metadata file
            string [] lines;
            lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Save\\metadata.data");
            return lines[0];
        }

        public Player LoadSave(){ // Returns a fully loaded player obj
            Player temp = BinarySerialization.ReadFromBinaryFile<Player>(Directory.GetCurrentDirectory() + "\\Save\\savedata.bin");
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
        public int SaveData(int index, string saveName, Player outPlayer){
            string outLocation = Directory.GetCurrentDirectory() + "\\Save\\metadata.data";
            using (StreamWriter sw = new StreamWriter(outLocation)){
                sw.WriteLine(saveName);
            }
            string binLocation = Directory.GetCurrentDirectory() + "\\Save\\savedata.bin";
            BinarySerialization.WriteToBinaryFile<Player>(binLocation, outPlayer);
            return 1;
        }
    }
}