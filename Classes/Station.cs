using System;
using Basiverse;
using Spectre.Console;
using System.Threading;

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
        public string[] ScienceServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
        public string[] MilitaryServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Undock"}; // Weapons Only
        public string[] TerminalServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Undock"}; // Sells ship upgrades too
        public string[] ReligiousServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
        public string[] ColonyServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
        public string[] CorporateServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
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
            AnsiConsole.Clear();
            DockScreen(inPlayer);
            ServicesMenu();
            return;
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
            // Ship info
            Table ShipScreen = new Table();
            ShipScreen.AddColumn("REPLACABLE COMPONENTS");
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Shield.Name} Cost: {inPlayer.PShip.Shield.Cost}"));
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Armor.Name} Cost: {inPlayer.PShip.Armor.Cost}"));
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Heatsink.Name} Cost: {inPlayer.PShip.Heatsink.Cost}"));
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Engine.Name} Cost: {inPlayer.PShip.Engine.Cost}"));
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Laser.Name} Cost: {inPlayer.PShip.Laser.Cost}"));
            ShipScreen.AddRow(new Markup($"Name: {inPlayer.PShip.Missile.Name} Cost: {inPlayer.PShip.Missile.Cost}"));
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

        public void ServicesMenu(){
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
                    // RepairMenu()
                break;
                case "Buy":
                    //BuyMenu();
                break;
                case "Sel":
                    //SellMenu();
                break;
                case "Upgrade":
                    //UpgradeMenu();
                break;
                case "Undock":
                    return;
            }
        }
    }
}