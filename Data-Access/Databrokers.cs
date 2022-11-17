using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Basiverse;
using Spectre.Console;

namespace Basiverse{

    /*
        This Databrokers file contains the Loader, Saver, Deleter, and Generator classes
    */

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
            if(File.Exists(Directory.GetCurrentDirectory() + "\\Save\\savedata.bin")){ // Check for save
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

    class Generator{
        public NPC GenerateCombatNPC(string name, int difficulty, int attitude){ // Genrates a random combat NPC of specified difficulty
            NPC tempNPC = new NPC(name, difficulty, attitude);
            var rand = new Random();
            // Get all item lists
            List<Chassis> Chassies = BinarySerialization.ReadFromBinaryFile<List<Chassis>>("Data//chassis.bin");
            double CSize = Chassies.Count;
            int CNum = 0;
            List<Shield> Shields = BinarySerialization.ReadFromBinaryFile<List<Shield>>("Data//shield.bin");
            double SSize = Shields.Count;
            int SNum = 0;
            List<Armor> Armors = BinarySerialization.ReadFromBinaryFile<List<Armor>>("Data//armor.bin");
            double ASize = Armors.Count;
            int ANum = 0;
            List<Heatsink> Heatsinks = BinarySerialization.ReadFromBinaryFile<List<Heatsink>>("Data//heatsink.bin");
            double HSize = Heatsinks.Count;
            int HNum = 0;
            List<Engine> Engines = BinarySerialization.ReadFromBinaryFile<List<Engine>>("Data//engine.bin");
            double ESize = Engines.Count;
            int ENum = 0;
            List<Missile> Missiles = BinarySerialization.ReadFromBinaryFile<List<Missile>>("Data//missile.bin");
            double MSize = Missiles.Count;
            int MNum = 0;
            int MStock = rand.Next(0, (2 * difficulty) + 1);
            List<Laser> Lasers = BinarySerialization.ReadFromBinaryFile<List<Laser>>("Data//laser.bin");
            double LSize = Lasers.Count;
            int LNum = 0;

            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data//shipnames.data");

            switch(difficulty){
                case 0: // 0 is the easiest, with all being default
                    CNum = 0;
                    SNum = 0;
                    ANum = 0;
                    HNum = 0;
                    ENum = 0;
                    MNum = 0;
                    LNum = 0;
                break;
                case 1: // Get the bottom 20% of the lists
                    CNum = rand.Next(0, Convert.ToInt32(Math.Ceiling(CSize/5)) + 1);
                    SNum = rand.Next(0, Convert.ToInt32(Math.Ceiling(SSize/5)) + 1);
                    ANum = rand.Next(0, Convert.ToInt32(Math.Ceiling(ASize/5)) + 1);
                    HNum = rand.Next(0, Convert.ToInt32(Math.Ceiling(HSize/5)) + 1);
                    ENum = rand.Next(0, Convert.ToInt32(Math.Ceiling(ESize/5)) + 1);
                    MNum = rand.Next(0, Convert.ToInt32(Math.Ceiling(MSize/5)) + 1);                    
                    LNum = rand.Next(0, Convert.ToInt32(Math.Ceiling(LSize/5)) + 1);
                break;
                case 2: // Then 20 - 40, etc.
                    CNum = rand.Next(Convert.ToInt32(Math.Ceiling(CSize/5)), Convert.ToInt32(Math.Ceiling(CSize/4)) + 1);
                    SNum = rand.Next(Convert.ToInt32(Math.Ceiling(SSize/5)), Convert.ToInt32(Math.Ceiling(SSize/4)) + 1);
                    ANum = rand.Next(Convert.ToInt32(Math.Ceiling(ASize/5)), Convert.ToInt32(Math.Ceiling(ASize/4)) + 1);
                    HNum = rand.Next(Convert.ToInt32(Math.Ceiling(HSize/5)), Convert.ToInt32(Math.Ceiling(HSize/4)) + 1);
                    ENum = rand.Next(Convert.ToInt32(Math.Ceiling(ESize/5)), Convert.ToInt32(Math.Ceiling(ESize/4)) + 1);
                    MNum = rand.Next(Convert.ToInt32(Math.Ceiling(MSize/5)), Convert.ToInt32(Math.Ceiling(MSize/4)) + 1);
                    LNum = rand.Next(Convert.ToInt32(Math.Ceiling(LSize/5)), Convert.ToInt32(Math.Ceiling(LSize/4)) + 1);
                break;
                case 3:
                    CNum = rand.Next(Convert.ToInt32(Math.Ceiling(CSize/4)), Convert.ToInt32(Math.Ceiling(CSize/3)) + 1);
                    SNum = rand.Next(Convert.ToInt32(Math.Ceiling(SSize/4)), Convert.ToInt32(Math.Ceiling(SSize/3)) + 1);
                    ANum = rand.Next(Convert.ToInt32(Math.Ceiling(ASize/4)), Convert.ToInt32(Math.Ceiling(ASize/3)) + 1);
                    HNum = rand.Next(Convert.ToInt32(Math.Ceiling(HSize/4)), Convert.ToInt32(Math.Ceiling(HSize/3)) + 1);
                    ENum = rand.Next(Convert.ToInt32(Math.Ceiling(ESize/4)), Convert.ToInt32(Math.Ceiling(ESize/3)) + 1);
                    MNum = rand.Next(Convert.ToInt32(Math.Ceiling(MSize/4)), Convert.ToInt32(Math.Ceiling(MSize/3)) + 1);
                    LNum = rand.Next(Convert.ToInt32(Math.Ceiling(LSize/4)), Convert.ToInt32(Math.Ceiling(LSize/3)) + 1);
                break;
                case 4:
                    CNum = rand.Next(Convert.ToInt32(Math.Ceiling(CSize/3)), Convert.ToInt32(Math.Ceiling(CSize/2)) + 1);
                    SNum = rand.Next(Convert.ToInt32(Math.Ceiling(SSize/3)), Convert.ToInt32(Math.Ceiling(SSize/2)) + 1);
                    ANum = rand.Next(Convert.ToInt32(Math.Ceiling(ASize/3)), Convert.ToInt32(Math.Ceiling(ASize/2)) + 1);
                    HNum = rand.Next(Convert.ToInt32(Math.Ceiling(HSize/3)), Convert.ToInt32(Math.Ceiling(HSize/2)) + 1);
                    ENum = rand.Next(Convert.ToInt32(Math.Ceiling(ESize/3)), Convert.ToInt32(Math.Ceiling(ESize/2)) + 1);
                    MNum = rand.Next(Convert.ToInt32(Math.Ceiling(MSize/3)), Convert.ToInt32(Math.Ceiling(MSize/2)) + 1);
                    LNum = rand.Next(Convert.ToInt32(Math.Ceiling(LSize/3)), Convert.ToInt32(Math.Ceiling(LSize/2)) + 1);
                break;
                case 5:
                    CNum = rand.Next(Convert.ToInt32(Math.Ceiling(CSize/2)), Convert.ToInt32(CSize) + 1);
                    SNum = rand.Next(Convert.ToInt32(Math.Ceiling(SSize/2)), Convert.ToInt32(SSize) + 1);
                    ANum = rand.Next(Convert.ToInt32(Math.Ceiling(ASize/2)), Convert.ToInt32(ASize) + 1);
                    HNum = rand.Next(Convert.ToInt32(Math.Ceiling(HSize/2)), Convert.ToInt32(HSize) + 1);
                    ENum = rand.Next(Convert.ToInt32(Math.Ceiling(ESize/2)), Convert.ToInt32(ESize) + 1);
                    MNum = rand.Next(Convert.ToInt32(Math.Ceiling(MSize/2)), Convert.ToInt32(MSize) + 1);
                    LNum = rand.Next(Convert.ToInt32(Math.Ceiling(LSize/2)), Convert.ToInt32(LSize) + 1);
                break;
            }
            // Generate ship with the random nums we got earlier and a random ship name
            Ship tmpShip = new Ship(NameLines[rand.Next(0, (NameLines.GetLength(0) + 1))], Chassies[CNum], Armors[ANum], Shields[SNum], Heatsinks[HNum], Lasers[LNum], Missiles[MNum], Engines[ENum]);
            tmpShip.Missile.Stock = MStock;
            tempNPC.cShip = tmpShip;
            return tempNPC;
        }
    }

    class LoreReader{
        public string GetNewGameLines(){
            return System.IO.File.ReadAllText("Data//Lore//intro.txt");
        }

        public string GetStartupLines(){
            return System.IO.File.ReadAllText("Data//Lore//startup.txt");
        }
    }

}