using System;
using System.IO;
using System.Linq;
using Basiverse;
using Spectre.Console;

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
            if(File.Exists(Directory.GetCurrentDirectory() + "\\Save\\savedata.bin")){ // Check for metadata and save
                return true;
            }
            else{
                return false;
            }
        }

        public Player LoadSave(){ // Returns a fully loaded player obj
            Player temp = BinarySerialization.ReadFromBinaryFile<Player>(Directory.GetCurrentDirectory() + "\\Save\\savedata.bin");
            return temp;
        }
    
    }

    class Saver{
        public int SaveData(Player outPlayer){
            string binLocation = Directory.GetCurrentDirectory() + "\\Save\\savedata.bin";
            BinarySerialization.WriteToBinaryFile<Player>(binLocation, outPlayer);
            return 1;
        }
    }

    class Deleter{
        public void DeleteData(){
            try{
                System.IO.DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Save\\");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete(); 
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true); 
                }
            }
            catch (Exception e){
                AnsiConsole.WriteException(e);
            }
        }
    }
}