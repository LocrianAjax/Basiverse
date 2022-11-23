using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;

namespace Basiverse
{
    [Serializable]
    class Player
    {
        // A player has a name, money, ship, and location
        private string _name;
        public string Name{ get {return _name;} set {_name = value;}}

        private double _money = 0; // Me too thanks
        public double Money{get {return _money;} set {_money = value;}}

        private int _jumpLog = 0; // Logs num jumps
        public int JumpLog{get {return _jumpLog;} set {_jumpLog = value;}}

        public Ship PShip;

        public Map PMap;

        public Location PLoc;
        
        public string CapitansLog = ""; // TODO: Add entry to log in starting system, when a jump is made, a combat is completed, and item is bought or sold or a station is visited

        public bool Rename(string newName){
            if(String.IsNullOrEmpty(newName) || String.IsNullOrWhiteSpace(newName)){
                return false; // Return false if there's no name
            }
            else{
                _name = newName;
                return true;
            }
        }

        public void BuyItem(Cargo inCargo){
            Table ItemTable = new Table(); // Table for the "Main" buy menu
            ItemTable.Title = new TableTitle($"BUYING {inCargo.Name}");
            ItemTable.AddColumns("NAME", "DESCRIPTION", "SIZE", "PRICE");
            ItemTable.AddRow($"{inCargo.Name}", $"{inCargo.Description}", $"{inCargo.Size}", $"{inCargo.AdjustedPrice}");
            AnsiConsole.Write(ItemTable);

            while(true){
                int amount = AnsiConsole.Ask<int>("How many would you like to purchase?:");
                if(amount == 0){
                    return;
                }
                else if((amount * inCargo.AdjustedPrice) > Money){
                    AnsiConsole.WriteLine("Please select an amount you can afford");
                }
                else if(PShip.CheckCargo(inCargo, amount)){
                    AnsiConsole.WriteLine("Please select an amount that fits in your hold");
                }
                else{
                    if(AnsiConsole.Confirm($"You would like to purchase {amount} of {inCargo.Name} for ${amount * inCargo.AdjustedPrice}?")){ // Price Check
                        PShip.AddCargo(inCargo, amount);
                        Money -= (amount * inCargo.AdjustedPrice);
                        return;   
                    }
                    else{
                        return;
                    }
                }
            }
        }

        public void SellItem(Cargo inCargo){
            Table ItemTable = new Table(); // Table for the "Main" Sell menu
            ItemTable.Title = new TableTitle($"SELLING {inCargo.Name}");
            ItemTable.AddColumns("NAME", "DESCRIPTION", "SIZE", "PRICE");
            ItemTable.AddRow($"{inCargo.Name}", $"{inCargo.Description}", $"{inCargo.Size}", $"{inCargo.AdjustedPrice}");
            AnsiConsole.Write(ItemTable);
            int avail = 0;
            foreach(Cargo tmp in PShip.CargoHold){
                if(tmp.Name == inCargo.Name){
                    avail++;
                }
            }

            while(true){
                int amount = AnsiConsole.Ask<int>("How many would you like to sell?:");
                if(amount == 0){
                    return;
                }
                else if(amount > avail){
                     AnsiConsole.WriteLine("Please select an amount equal to or less that the amount you have.");
                }
                else{
                    if(AnsiConsole.Confirm($"You would like to sell {amount} of {inCargo.Name} for ${amount * inCargo.AdjustedPrice}?")){ // Price Check
                        PShip.RemoveCargo(inCargo, amount);
                        Money += (amount * inCargo.AdjustedPrice);
                        return;   
                    }
                    else{
                        return;
                    }
                }
            }
        }

        public void UpgradeChassis(Chassis inChassis){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inChassis.Name}", $"{inChassis.Description}", $"{inChassis.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your chassis for ${inChassis.Cost}?")){
                    if(inChassis.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.UpgradeChassis(inChassis);
                        Money -= inChassis.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }

        public void UpgradeShield(Shield inShield){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "PRICE");
            UpgradeTable.AddRow($"{inShield.Name}", $"{inShield.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your shield for ${inShield.Cost}?")){
                    if(inShield.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your shield, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Shield = inShield;
                        Money -= inShield.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }

        public void UpgradeArmor(Armor inArmor){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inArmor.Name}", $"{inArmor.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your armor for ${inArmor.Cost}?")){
                    if(inArmor.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Armor = inArmor;
                        Money -= inArmor.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }
        public void UpgradeHeatsink(Heatsink inHeatsink){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inHeatsink.Name}", $"{inHeatsink.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your heatsink for ${inHeatsink.Cost}?")){
                    if(inHeatsink.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Heatsink = inHeatsink;
                        Money -= inHeatsink.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }
        public void UpgradeEngine(Engine inEngine){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inEngine.Name}", $"{inEngine.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your engine for ${inEngine.Cost}?")){
                    if(inEngine.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Engine = inEngine;
                        Money -= inEngine.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }
        public void UpgradeMissiles(Missile inMissiles){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inMissiles.Name}", $"{inMissiles.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your missiles for ${inMissiles.Cost}?")){
                    if(inMissiles.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Missile = inMissiles;
                        Money -= inMissiles.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }
        public void UpgradeLasers(Laser inLasers){
            Table UpgradeTable = new Table(); // Table for the "Main" buy menu
            UpgradeTable.Title = new TableTitle($"CHASSIS UPGRADE");
            UpgradeTable.AddColumns("NAME", "DESCRIPTION", "PRICE");
            UpgradeTable.AddRow($"{inLasers.Name}", $"{inLasers.Cost}");
            AnsiConsole.Write(UpgradeTable);

                if(AnsiConsole.Confirm($"Would you like to upgrade your lasers for ${inLasers.Cost}?")){
                    if(inLasers.Cost > Money){
                        AnsiConsole.Prompt(new TextPrompt<string>("You cannot afford to upgrade your chassis, Press Enter to continue...").AllowEmpty());
                        return;
                    }
                    else{
                        PShip.Laser = inLasers;
                        Money -= inLasers.Cost;
                        return;   
                    }
                }
                else{
                    return;
                }
        }

        public void UpgradeSystem(string sysType){

            string buyOpts = "";
            switch(sysType){
                case "Chassis":
                    List<Chassis> Chassies = BinarySerialization.ReadFromBinaryFile<List<Chassis>>("Data//chassis.bin");
                    foreach(Chassis tmp in Chassies){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Shield":
                    List<Shield> Shields = BinarySerialization.ReadFromBinaryFile<List<Shield>>("Data//shield.bin");
                    foreach(Shield tmp in Shields){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Armor":
                    List<Armor> Armors = BinarySerialization.ReadFromBinaryFile<List<Armor>>("Data//armor.bin");
                    foreach(Armor tmp in Armors){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Heatsink":
                    List<Heatsink> Heatsinks = BinarySerialization.ReadFromBinaryFile<List<Heatsink>>("Data//heatsink.bin");
                    foreach(Heatsink tmp in Heatsinks){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Engine":
                    List<Engine> Engines = BinarySerialization.ReadFromBinaryFile<List<Engine>>("Data//engine.bin");
                    foreach(Engine tmp in Engines){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Missiles":
                    List<Missile> Missiles = BinarySerialization.ReadFromBinaryFile<List<Missile>>("Data//missile.bin");
                    foreach(Missile tmp in Missiles){
                        buyOpts += tmp.Name + "|";
                    }
                break;
                case "Lasers":
                    List<Laser> Lasers = BinarySerialization.ReadFromBinaryFile<List<Laser>>("Data//laser.bin");
                    foreach(Laser tmp in Lasers){
                        buyOpts += tmp.Name + "|";
                    }
                break;
            }

            buyOpts += "Return";
            string[] options = buyOpts.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}

            string itemName = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an Upgrade:")
            .PageSize(pageCount)
            .AddChoices(options));
            
            switch(sysType){
                case "Chassis":
                    List<Chassis> Chassies = BinarySerialization.ReadFromBinaryFile<List<Chassis>>("Data//chassis.bin");
                    foreach(Chassis tmp in Chassies){
                        if(tmp.Name == itemName){
                            UpgradeChassis(tmp);
                            break;
                        }
                    }
                break;
                case "Shield":
                    List<Shield> Shields = BinarySerialization.ReadFromBinaryFile<List<Shield>>("Data//shield.bin");
                    foreach(Shield tmp in Shields){
                        if(tmp.Name == itemName){
                            UpgradeShield(tmp);
                            break;
                        }
                    }
                break;
                case "Armor":
                    List<Armor> Armors = BinarySerialization.ReadFromBinaryFile<List<Armor>>("Data//armor.bin");
                    foreach(Armor tmp in Armors){
                        if(tmp.Name == itemName){
                            UpgradeArmor(tmp);
                            break;
                        }
                    }
                break;
                case "Heatsink":
                    List<Heatsink> Heatsinks = BinarySerialization.ReadFromBinaryFile<List<Heatsink>>("Data//heatsink.bin");
                    foreach(Heatsink tmp in Heatsinks){
                        if(tmp.Name == itemName){
                            UpgradeHeatsink(tmp);
                            break;
                        }
                    }
                break;
                case "Engine":
                    List<Engine> Engines = BinarySerialization.ReadFromBinaryFile<List<Engine>>("Data//engine.bin");
                    foreach(Engine tmp in Engines){
                        if(tmp.Name == itemName){
                            UpgradeEngine(tmp);
                            break;
                        }
                    }
                break;
                case "Missiles":
                    List<Missile> Missiles = BinarySerialization.ReadFromBinaryFile<List<Missile>>("Data//missile.bin");
                    foreach(Missile tmp in Missiles){
                        if(tmp.Name == itemName){
                            UpgradeMissiles(tmp);
                            break;
                        }
                    }
                break;
                case "Lasers":
                    List<Laser> Lasers = BinarySerialization.ReadFromBinaryFile<List<Laser>>("Data//laser.bin");
                    foreach(Laser tmp in Lasers){
                        if(tmp.Name == itemName){
                            UpgradeLasers(tmp);
                            break;
                        }
                    }
                break;
                case "Return":
                    return;
            }
        }
        public void RestockMissiles(int missilePrice){
            while(true){
                AnsiConsole.WriteLine($"You currently have: {PShip.Missile.Stock} missiles");
                int amount = AnsiConsole.Ask<int>("How many missiles would you like to purchase?:");
                if(amount == 0){
                    return;
                }
                else if((amount * missilePrice) > Money){
                    AnsiConsole.WriteLine("Please select an amount you can afford");
                }
                else if(amount < 0){
                    AnsiConsole.WriteLine("Please select a non-negative amount");
                }
                else{
                    if(AnsiConsole.Confirm($"You would like to purchase {amount} of missiles for ${amount * missilePrice}?")){ // Price Check
                        PShip.Missile.Stock += amount;
                        Money -= (amount * missilePrice);
                        return;   
                    }
                    else{
                        return;
                    }
                }
            }
        }
    
        public int GetDifficulty(){ // Returns the combat difficulty based on number of jumps
            if(_jumpLog < 100){
                return 0;
            }
            else{
                return Convert.ToInt32((Math.Floor(Convert.ToDouble(_jumpLog / 100))));
            }
        }
    
        public void combatRewards(int difficulty){
            int rewardsMoney = 0;
            var rand = new Random();
            switch(difficulty){
                case 0:
                    rewardsMoney = rand.Next(0, 50);
                break;
                case 1:
                    rewardsMoney = rand.Next(50, 150);
                break;
                case 2:
                    rewardsMoney = rand.Next(150, 250);
                break;
                case 3:
                    rewardsMoney = rand.Next(250, 350);
                break;
                case 4:
                    rewardsMoney = rand.Next(350, 500);
                break;
                case 5:
                    rewardsMoney = rand.Next(500, 1000);
                break;
            }
            AnsiConsole.Write($"Bounty cash recieved: {rewardsMoney}$");
            _money = _money + rewardsMoney;
        }
    
        public void logStart(){
            CapitansLog += $"Spawned in {PLoc.Name}\n";            
        }

        public void logJump(string dest){
            CapitansLog += $"Jumped from {PLoc.Name} to {dest}\n";
        }

        public void logDock(string stationName){
            CapitansLog += $"Docked at {stationName}\n";
        }

        public void logVictory(string enemyName){
            CapitansLog += $"Defeated The {enemyName} in battle\n";
        }

        public void logFlee(string enemyName, bool pFled){
            if(pFled){
                CapitansLog += $"Fled from a battle with The {enemyName}\n";
            }
            else{
                CapitansLog += $"The {enemyName} fled from battle\n";
            }
        }
    
        public void displayLog(){
            string[] lines = CapitansLog.Split('\n');
            Table capitansLog = new Table();
            capitansLog.AddColumn($"CAPITANS LOG: {Name}");
            foreach(string line in lines){
                capitansLog.AddRow(line);
            }
            capitansLog.Expand();
            AnsiConsole.Write(capitansLog);
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue........").AllowEmpty());
        }
    }
}
