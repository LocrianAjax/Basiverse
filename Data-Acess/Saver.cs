using System;
using System.IO;
using System.Linq;
using Basiverse;

namespace Basiverse
{
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
            Locations[0] = Directory.GetCurrentDirectory() + "\\save1\\";
            Locations[1] = Directory.GetCurrentDirectory() + "\\save2\\";
            Locations[2] = Directory.GetCurrentDirectory() + "\\save3\\";
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