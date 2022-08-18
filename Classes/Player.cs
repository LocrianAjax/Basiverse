using System;
using Basiverse;
using Spectre.Console;

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
        
        public String CapitansLog; // TODO

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
                else if(PShip.CheckCargo(inCargo, amount) == -1){
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
    }
}
