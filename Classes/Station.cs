using System;
using Basiverse;
using Spectre.Console;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace Basiverse{

    [Serializable]
    class Station{
        // A station has a name, a type, a list of NPCs
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private string _type = ""; // basic, science, military, terminal, religous, colony, corporate, wreck 
        public string Type{ get {return _type;} set {_type = value;}}
        private string _description = "";
        public string Description{ get {return _description;} set {_description = value;}}

        private double _repairCost = 1; // Default to 1 space dollar per hull point
        public double RepairCost{ get {return _repairCost;} set {_repairCost = value;}}
        /*
            Services - 
            All stations offer repairing and some shopping, except for wrecks. But the contents differ per location
            Basic - Buys all goods, but for a reduced price. Sells Basic/Recreation/Basic Luxury
            Science - Buys all goods with a premium put on Science/Industiral goods. Sells Science and repairs for cheaper.
            Military - Does not buy Science/Basic/Industrial/Luxury, with a premium on basic luxury goods and recreatiuon. Sells weapons for cheap.
            Terminal - Buys and sells everything, prices vary based on market conditions. 
            Religious - Does not buy Science/Basic/Industrial/Luxury, with a premium on basic luxury goods and recreatiuon. Doesn't provide addl services
            Colony - Does not buy Luxury, with a premium on basic, basic luxury goods and recreatiuon.
            Corporate - Buys and sells everything, prices vary based on market conditions.
            Wreck - Nothing, Nothing, Tra-la-la?
        
        */
        public string[] BasicServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Undock"}; 
        public string[] ScienceServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; // Cheaper repairs
        public string[] MilitaryServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Undock"}; // Weapons Only
        public string[] TerminalServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Undock"}; // Sells ship upgrades too, repairs vairy by market cost
        public string[] ReligiousServices = new string[]{"Buy", "Sell", "Undock"}; 
        public string[] ColonyServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
        public string[] CorporateServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; // Repairs vairy by market cost
        public string[] WreckServices = new string[]{"Nothing, nothing tra-la-la?!", "Undock"}; 

        public Station(){ // Default constructor
            _name = "Derelict";
            _type = "Wreck";
            _description = "A former station now abandoned.";
        }

        public Station(string inName, string inType, string inDescription){ // Overloaded constructor
            _name = inName;
            _type = inType;
            _description = inDescription;
        }

        public void Dock(Player inPlayer){
            int retval = 0;
            inPlayer.PShip.RestoreShields(); // Fully restore the shield on docking
            while(retval == 0){
                AnsiConsole.Clear();
                DockScreen(inPlayer);
                retval = ServicesMenu(inPlayer);
            }
        }

        public void DockScreen(Player inPlayer){
            /*
                Ship Manifest    |     Station Data
                System : Cost           Desc and shit
                Cargo List
            */
            Table DockingScreen = new Table(); 
            DockingScreen.Expand();
            DockingScreen.AddColumns("SHIP MANIFEST", "STATION DATA");

            // Set up Capitan Info
            Table CapitanScreen = new Table();
            CapitanScreen.AddColumn($"Welcome Capitan {inPlayer.Name}!");
            CapitanScreen.AddRow($"Available Money: ${inPlayer.Money}");
            CapitanScreen.AddRow("Log Data:");
            CapitanScreen.AddRow($"Jumps: {inPlayer.JumpLog}");

            Table ShipInfo = new Table();
            ShipInfo.AddColumn("SHIP MANIFEST");
            ShipInfo.AddRow($"Name: {inPlayer.PShip.Shield.Name} Cost: {inPlayer.PShip.Shield.Cost}");
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Armor.Name} Cost: {inPlayer.PShip.Armor.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Heatsink.Name} Cost: {inPlayer.PShip.Heatsink.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Engine.Name} Cost: {inPlayer.PShip.Engine.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Laser.Name} Cost: {inPlayer.PShip.Laser.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Missile.Name} Cost: {inPlayer.PShip.Missile.Cost}"));

            // Ship info
            Table ShipScreen = new Table();
            ShipScreen.AddColumns("REPLACABLE COMPONENTS", "CAPITAN'S INFO");
            ShipScreen.AddRow(ShipInfo, CapitanScreen);

            // Cargo Table
            Table CargoScreen = new Table();
            CargoScreen.Title = new TableTitle("CARGO MANIFEST");
            if(inPlayer.PShip.Hold.HoldItems != null){
                CargoScreen.AddColumns("ITEM","COST");
                foreach(Cargo item in inPlayer.PShip.Hold.HoldItems){
                    CargoScreen.AddRow(new Markup($"Name: {item.Name}"), new Markup($"Cost: {item.Cost}"));
                }
            }
            else{
                CargoScreen.AddColumn("HOLD EMPTY");
            }

            // Set up station info
            Table StationScreen = new Table();
            StationScreen.Title = new TableTitle(Name);
            StationScreen.AddColumn($"Welcome to {Name}! A {Type} Staion");
            StationScreen.AddRow(Description);

            DockingScreen.AddRow(ShipScreen, StationScreen);
            DockingScreen.AddRow(CargoScreen);

            AnsiConsole.Write(DockingScreen);
        }

        public int ServicesMenu(Player inPlayer){
            string selection = "";
            switch(Type){ // basic, science, military, terminal, religous, colony, corporate, wreck 
                case "Basic":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(BasicServices));
                break;
                case "Science":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(ScienceServices));
                break;
                case "Military":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(MilitaryServices));
                break;
                case "Terminal":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(TerminalServices));
                break;
                case "Religious":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(ReligiousServices));
                break;
                case "Colony":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(ColonyServices));
                break;
                case "Corporate":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(CorporateServices));
                break;
                case "Wreck":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(5)
                    .AddChoices(WreckServices));
                break;
            }
            
            switch(selection){
                case "Repair":
                    RepairMenu(inPlayer);
                break;
                case "Buy":
                    BuyMenu(inPlayer);
                break;
                case "Sell":
                    SellMenu(inPlayer);
                break;
                case "Upgrade":
                    //UpgradeMenu(Player inPlayer);
                break;
                case "Nothing, nothing tra-la-la?!":
                    GoblinKing();
                break;
                case "Undock":
                    return 1;
            }
            
            return 0;
        }

        public void RepairMenu(Player inPlayer){
            /*
                (station name) REPAIR SERVICES
                | Ship Name | Hull val / Max|
                | Max Repair amount| Cost |

            */
            double repairAmount = (inPlayer.PShip.Hull.HullMax - inPlayer.PShip.Hull.Hullval);
            var rand = new Random();

            switch(Type){
                case "Science":
                    _repairCost = .8;
                break;
                case "Terminal": // These two have a random chance to be from .5 to 5
                    _repairCost = (.5 * rand.Next(1,10));
                break;
                case "Corporate":
                    _repairCost = (.5 * rand.Next(1,10));
                break;
                default: // Otherwise keep it as default
                break;
            }
            // Set up repair
            Table RepairScreen = new Table();
            RepairScreen.Title = new TableTitle($"{Name} REPAIR SERVICES");
            RepairScreen.AddColumns("VESSEL", "HULL INFO");
            RepairScreen.AddRow($"{inPlayer.PShip.Name}", $"Curent Hull Value: {inPlayer.PShip.Hull.Hullval} Maximum: {inPlayer.PShip.Hull.HullMax}");
            RepairScreen.AddRow($"Maximum Repairable: {repairAmount}", $"Cost: {RepairCost} per Hull Point");
            AnsiConsole.Write(RepairScreen);

            while(true){
                int amount = AnsiConsole.Ask<int>("How many points would you like to repair?:");
                if(amount == 0){
                    return;
                }
                else if(amount > repairAmount || amount < 0){
                    AnsiConsole.WriteLine("Please choose an amount greater than 0 or less than the max");
                }
                else if((amount * RepairCost) > inPlayer.Money){
                    AnsiConsole.WriteLine("Please select an amount you can afford");
                }
                else{
                    if(AnsiConsole.Confirm($"Would you like to repair your ship {amount} points for ${ Math.Floor(amount * RepairCost)}?")){ // Price Check
                        inPlayer.PShip.Repair(amount);
                        inPlayer.Money -= Convert.ToInt32(Math.Floor(amount * RepairCost));
                        return;   
                    }
                    else{
                        return;
                    }
                }
            }
            
        }

        public void BuyMenu(Player inPlayer){
            List<Cargo> Cargos = BinarySerialization.ReadFromBinaryFile<List<Cargo>>("Data\\cargo.bin");
        }

        public void SellMenu(Player inPlayer){
            List<Cargo> Cargos = BinarySerialization.ReadFromBinaryFile<List<Cargo>>("Data\\cargo.bin");
        }

        public void GoblinKing(){
            AnsiConsole.Clear();
            var image = new CanvasImage("//Data//david-bowie-labyrinth.jpg");
            var tmp = AnsiConsole.Prompt(
                    new TextPrompt<string>("")
                    .AllowEmpty());
        }
        // TODO: Service Menus
    }
}