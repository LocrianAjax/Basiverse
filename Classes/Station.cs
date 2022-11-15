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

        // A station may contain a list of NPCs
        // TODO: public List<NPC> NPCList;

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
        public string[] BasicServices = new string[]{"Repair", "Buy", "Sell", "Undock"};
        public string[] ScienceServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; // Cheaper repairs
        public string[] MilitaryServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Restock", "Undock"}; // Weapons Only for upgrade
        public string[] TerminalServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Restock", "Undock"}; // Sells ship upgrades too, repairs vairy by market cost
        public string[] ReligiousServices = new string[]{"Sell", "Undock"}; 
        public string[] ColonyServices = new string[]{"Repair", "Buy", "Sell", "Undock"}; 
        public string[] CorporateServices = new string[]{"Repair", "Buy", "Sell", "Upgrade", "Restock", "Undock"}; // Repairs vairy by market cost
        public string[] WreckServices = new string[]{"Nothing, nothing tra-la-la?!", "Undock"}; 

        // Multilplying for buying/selling at stations
        public double BaseBuyMult = 1;
        public double T0BuyMult = 1;
        public double T1BuyMult = 1;
        public double T2BuyMult = 1;
        public double T3BuyMult = 1;
        public double T4BuyMult = 1;
        public double T5BuyMult = 1;
        public double T6BuyMult = 1;
        public double T7BuyMult = 1; // Can only be sold

        public double BaseSellMult = 1;
        public double T0SellMult = 1;
        public double T1SellMult = 1;
        public double T2SellMult = 1;
        public double T3SellMult = 1;
        public double T4SellMult = 1;
        public double T5SellMult = 1;
        public double T6SellMult = 1;

        public List<Cargo> Cargos = new List<Cargo>();
        public List<int> StationSellList = new List<int>();
        public string buyOpts = "";
        public List<int> StationBuyList = new List<int>();
        public string sellOpt = "";
        public List<Cargo> forSale = new List<Cargo>();

        public List<Cargo> toBuy = new List<Cargo>();

        public Station(){ // Default constructor
            _name = "Derelict";
            _type = "Wreck";
            _description = "A former station now abandoned.";
        }

        public Station(string inName, string inType, string inDescription){ // Overloaded constructor
            _name = inName;
            _type = inType;
            _description = inDescription;

            Cargos = BinarySerialization.ReadFromBinaryFile<List<Cargo>>("Data//cargo.bin");
            var rand = new Random();

            /*
            Type 0 - Basic
            Type 1 - Recreation
            Type 2 - Basic Luxury
            Type 3 - Drugs and Booze
            Type 4 - High End Luxury Goods
            Type 5 - Industial Goods
            Type 6 - Science Equipment
            */

            switch(Type){ // Set up buy and sell lists
                case "Basic":
                    // Sells Basic/Recreation/Basic Luxury
                    StationSellList.Add(0);
                    StationSellList.Add(1);
                    StationSellList.Add(2);
                    // Buys everything for a reduced price
                    BaseBuyMult = .7;
                    for(int i = 0; i < 7; i++){
                        StationBuyList.Add(i);
                    }
                break;
                case "Science":
                    // Sells Science for a slightly lower cost
                    StationSellList.Add(6);
                    T6SellMult = .9;
                    // Buys all goods with a premium put on Science/Industiral goods
                    for(int i = 0; i < 7; i++){
                        StationBuyList.Add(i);
                    }
                    T5BuyMult = 1.4;
                    T6BuyMult = 2.5;
                    T7BuyMult = 2.5;
                break;
                case "Military":
                    // Sells Basic for an inflated price
                    StationSellList.Add(0);
                    T0SellMult = 1.2;
                    // Does not buy Science/Basic/Industrial/Luxury, a premium on basic luxury goods and recreation and even more for drugs/booze
                    StationBuyList.Add(1);
                    StationBuyList.Add(2);
                    StationBuyList.Add(3);
                    T1BuyMult = 2;
                    T2BuyMult = 1.5;
                    T3BuyMult = 3;
                break;
                case "Religous": 
                    // Sells nothing
                    // Does not buy Science/Basic/Industrial/Luxury, with a premium on basic luxury goods and recreatiuon
                    StationBuyList.Add(1);
                    StationBuyList.Add(2);
                    StationBuyList.Add(3);
                    T1BuyMult = 1.75;
                    T2BuyMult = 1.4;
                break;
                case "Colony":
                    // Sells Industiral for a slighly lower price
                    StationSellList.Add(5);
                    T5SellMult = .9;
                    // Does not buy Luxury, with a premium on basic, basic luxury goods, recreation and cheap booze
                    StationBuyList.Add(1);
                    StationBuyList.Add(2);
                    StationBuyList.Add(3);
                    StationBuyList.Add(5);
                    StationBuyList.Add(6);
                    StationBuyList.Add(7);
                    T0BuyMult = 2.5;
                    T1BuyMult = 2;
                    T2BuyMult = 1.5;
                    T3BuyMult = 3;
                break;
                case "Terminal":
                    // Sells everything at a variable cost
                    for(int i = 0; i <= 6; i++){
                        StationSellList.Add(i);
                    }
                    // Everything has a random chance to be multiplied from .5 to 5
                    BaseSellMult = (.5 * rand.Next(1,10));
                    T0SellMult = (.5 * rand.Next(1,10));
                    T1SellMult = (.5 * rand.Next(1,10));
                    T2SellMult = (.5 * rand.Next(1,10));
                    T3SellMult = (.5 * rand.Next(1,10));
                    T4SellMult = (.5 * rand.Next(1,10));
                    T5SellMult = (.5 * rand.Next(1,10));
                    T6SellMult = (.5 * rand.Next(1,10));
                    // Buys everything at a variable cost
                    for(int i = 0; i <= 7; i++){
                        StationBuyList.Add(i);
                    }
                    // Everything has a random chance to be multiplied from .5 to 5
                    BaseBuyMult = (.5 * rand.Next(1,10));
                    T0BuyMult = (.5 * rand.Next(1,10));
                    T1BuyMult = (.5 * rand.Next(1,10));
                    T2BuyMult = (.5 * rand.Next(1,10));
                    T3BuyMult = (.5 * rand.Next(1,10));
                    T4BuyMult = (.5 * rand.Next(1,10));
                    T5BuyMult = (.5 * rand.Next(1,10));
                    T6BuyMult = (.5 * rand.Next(1,10));
                    T7BuyMult = (.5 * rand.Next(1,10));
                break;
                case "Corporate":
                    // Sells everything at a variable cost
                    for(int i = 0; i <= 6; i++){
                        StationSellList.Add(i);
                    }
                    // Everything has a random chance to be multiplied from .5 to 5
                    BaseSellMult = (.5 * rand.Next(1,10));
                    T0SellMult = (.5 * rand.Next(1,10));
                    T1SellMult = (.5 * rand.Next(1,10));
                    T2SellMult = (.5 * rand.Next(1,10));
                    T3SellMult = (.5 * rand.Next(1,10));
                    T4SellMult = (.5 * rand.Next(1,10));
                    T5SellMult = (.5 * rand.Next(1,10));
                    T6SellMult = (.5 * rand.Next(1,10));
                    // Buys everything at a variable cost
                    for(int i = 0; i <= 6; i++){
                            StationBuyList.Add(i);
                    }
                    // Everything has a random chance to be multiplied from .5 to 5
                    BaseBuyMult = (.5 * rand.Next(1,10));
                    T0BuyMult = (.5 * rand.Next(1,10));
                    T1BuyMult = (.5 * rand.Next(1,10));
                    T2BuyMult = (.5 * rand.Next(1,10));
                    T3BuyMult = (.5 * rand.Next(1,10));
                    T4BuyMult = (.5 * rand.Next(1,10));
                    T5BuyMult = (.5 * rand.Next(1,10));
                    T6BuyMult = (.5 * rand.Next(1,10));
                    T7BuyMult = (.5 * rand.Next(1,10));
                break;
                default: // Otherwise keep it as default
                break;
            }

            foreach(Cargo tmp in Cargos){ // Set up buy/sell lists
                if(StationSellList.Contains(tmp.Type)){
                    forSale.Add(tmp);
                }

                if(StationBuyList.Contains(tmp.Type)){
                    toBuy.Add(tmp);
                }
            }

            foreach(int i in StationSellList){
                switch(i){
                    case 0:
                        buyOpts += "Basic Goods|";
                    break;
                    case 1:
                        buyOpts += "Recreational Goods|";
                    break;
                    case 2:
                        buyOpts += "Luxury Items|";
                    break;
                    case 3:
                        buyOpts += "Drugs and Alcohol|";
                    break;
                    case 4:
                        buyOpts += "High End Luxury|";
                    break;
                    case 5:
                        buyOpts += "Industrial Goods|";
                    break;
                    case 6:
                        buyOpts += "Science Equipment|";
                    break;
                }
            }
            buyOpts += "Return";
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
            ShipInfo.AddRow($"SHIELD: {inPlayer.PShip.Shield.Name}");
            ShipInfo.AddRow(new Markup($"ARMOR: {inPlayer.PShip.Armor.Name}"));
            ShipInfo.AddRow(new Markup($"HEATSINK: {inPlayer.PShip.Heatsink.Name}"));
            ShipInfo.AddRow(new Markup($"ENGINE: {inPlayer.PShip.Engine.Name}"));
            ShipInfo.AddRow(new Markup($"LASERS: {inPlayer.PShip.Laser.Name}"));
            ShipInfo.AddRow(new Markup($"MISSILES: {inPlayer.PShip.Missile.Name}"));         

            // Ship info
            Table ShipScreen = new Table();
            ShipScreen.AddColumns("REPLACABLE COMPONENTS", "CAPITAN'S INFO");
            ShipScreen.AddRow(ShipInfo, CapitanScreen);
            ShipScreen.AddRow($"Cargo Hold: {inPlayer.PShip.Hold.Name} Free Space: {inPlayer.PShip.Hold.MaxSize - inPlayer.PShip.Hold.CurrentSize}M^3 Used Space: {inPlayer.PShip.Hold.CurrentSize}M^3");

            // Cargo Table
            Table CargoScreen = new Table();
            CargoScreen.Title = new TableTitle("CARGO MANIFEST");
            if(inPlayer.PShip.CargoHold != null){
                CargoScreen.AddColumns("ITEM","COST", "SIZE");
                foreach(Cargo item in inPlayer.PShip.CargoHold){
                    CargoScreen.AddRow($"Name: {item.Name}", $"Cost: {item.Cost}", $"Size: {item.Size}");
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
                    .PageSize(6)
                    .AddChoices(BasicServices));
                break;
                case "Science":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(ScienceServices));
                break;
                case "Military":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(MilitaryServices));
                break;
                case "Terminal":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(TerminalServices));
                break;
                case "Religious":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(ReligiousServices));
                break;
                case "Colony":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(ColonyServices));
                break;
                case "Corporate":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(CorporateServices));
                break;
                case "Wreck":
                    selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Services:")
                    .PageSize(6)
                    .AddChoices(WreckServices));
                break;
            }
            
            switch(selection){
                case "Repair":
                    RepairMenu(inPlayer);
                    for(int i = 0; i < 6; i++){

                    }
                break;
                case "Buy":
                    BuyMenu(inPlayer);
                break;
                case "Sell":
                    SellMenu(inPlayer);
                break;
                case "Upgrade":
                    UpgradeMenu(inPlayer);
                break;
                case "Restock":
                    RestockMenu(inPlayer);
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
            double repairAmount = Math.Floor(inPlayer.PShip.Hull.HullMax - inPlayer.PShip.Hull.Hullval);
            var rand = new Random();

            switch(Type){
                case "Science":
                    _repairCost = .8;
                break;
                case "Terminal": // These two have a random chance to be from .5 to 5
                    _repairCost = Math.Floor(.5 * rand.Next(1,10));
                break;
                case "Corporate":
                    _repairCost = Math.Floor(.5 * rand.Next(1,10));
                break;
                default: // Otherwise keep it as default
                break;
            }
            // Set up repair
            Table RepairScreen = new Table();
            RepairScreen.Title = new TableTitle($"{Name} REPAIR SERVICES");
            RepairScreen.AddColumns("VESSEL", "HULL INFO");
            RepairScreen.AddRow($"The {inPlayer.PShip.Name}", $"Curent Hull Value: {inPlayer.PShip.Hull.Hullval} Maximum: {inPlayer.PShip.Hull.HullMax}");
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


        public void BuyMenu(Player inPlayer){ // Broken into 3 functions, Selecting a type, selecting 
            /*
                (station name) MARKET - BUY
            */
            string items = "";
            AnsiConsole.Clear();
            Table MarketTable = new Table(); // Table for the "Main" buy menu
            MarketTable.Title = new TableTitle($"{Name} MARKET - BUY");
            MarketTable.AddColumns("CARGO MANIFEST","SHOP INFORMATION");
            
            // Add Cargo Info
            Table InfoTable = new Table();
            InfoTable.Title = new TableTitle($"FREE: {inPlayer.PShip.Hold.MaxSize - inPlayer.PShip.Hold.CurrentSize}M^3 CASH: ${inPlayer.Money}");
            if(inPlayer.PShip.CargoHold != null){
                InfoTable.AddColumns("ITEM","COST");
                foreach(Cargo item in inPlayer.PShip.CargoHold){
                    switch(item.Type){ // List the 'Buy' prices here, since that's what the station will pay
                    case 0:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T0BuyMult;
                    break;
                    case 1:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T1BuyMult;
                    break;
                    case 2:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T2BuyMult;
                    break;
                    case 3:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T3BuyMult;
                    break;
                    case 4:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T4BuyMult;
                    break;
                    case 5:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T5BuyMult;
                    break;
                    case 6:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T6BuyMult;
                    break;
                    }
                    InfoTable.AddRow(new Markup($"Name: {item.Name}"), new Markup($"Cost: {item.AdjustedPrice}"));
                }
            }
            else{
                InfoTable.AddColumn("HOLD EMPTY");
            }

            // Add Shop Info
            Table BuyTable = new Table(); 
            BuyTable.AddColumns("NAME", "DESCRIPTION", "SIZE", "PRICE");
            int type = BuyTypeMenu(); // Figure out which page we're going in

            if(type == -1){return;}

            foreach(Cargo tmp in forSale){ // Adjust the price based on the type now to avoid repeating
                if(tmp.Type == type){
                    items += tmp.Name + "|";
                    switch(type){
                    case 0:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T0SellMult;
                    break;
                    case 1:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T1SellMult;
                    break;
                    case 2:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T2SellMult;
                    break;
                    case 3:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T3SellMult;
                    break;
                    case 4:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T4SellMult;
                    break;
                    case 5:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T5SellMult;
                    break;
                    case 6:
                        tmp.AdjustedPrice = tmp.Cost * BaseSellMult * T6SellMult;
                    break;
                    }
                    BuyTable.AddRow($"{tmp.Name}", $"{tmp.Description}", $"{tmp.Size}", $"{tmp.AdjustedPrice}");
                }
            }

            MarketTable.AddRow(InfoTable, BuyTable);
            MarketTable.Expand();
            AnsiConsole.Write(MarketTable);

            items += "Return";
            string[] options = items.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}

            string itemName = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an Item:")
            .PageSize(pageCount)
            .AddChoices(options));
            
            if(itemName == "Return"){
                return;
            }
            else{
                foreach(Cargo buying in forSale){ // Once you select the item to buy, push down into that menu
                    if(buying.Name == itemName){
                        inPlayer.BuyItem(buying);
                    }
                }
            }
            return;
        }


        public int BuyTypeMenu(){ // Use this menu to select the type of items to buy
            string[] options = buyOpts.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}
            string page = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a Category:")
                .PageSize(pageCount)
                .AddChoices(options));

            if(page == "Return"){
                return -1;
            }
            else{
                switch(page){
                    case "Basic Goods":
                       return 0;
                    case "Recreational Goods":
                        return 1;
                    case "Luxury Items":
                        return 2;
                    case "Drugs and Alcohol":
                        return 3;
                    case "High End Luxury":
                        return 4;
                    case "Industrial Goods":
                        return 5;
                    case "Science Equipment":
                        return 6;
                    default:
                        return -1;
                }
            }
        }

        public void SellMenu(Player inPlayer){ // Selling from the Player to the station
            /*
                (station name) MARKET - SELL
                List Cargo on board and price
            */
            Table SellTable = new Table();
            SellTable.Title = new TableTitle($"{Name} MARKET - SELL");
            SellTable.AddColumns("CARGO MANIFEST", "STATION WILL BUY");
            // Cargo Table
            Table CargoScreen = new Table();
            if(inPlayer.PShip.CargoHold != null){
                CargoScreen.AddColumns("ITEM","COST");
                foreach(Cargo item in inPlayer.PShip.CargoHold){
                    switch(item.Type){ // List the 'Buy' prices here, since that's what the station will pay
                    case 0:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T0BuyMult;
                    break;
                    case 1:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T1BuyMult;
                    break;
                    case 2:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T2BuyMult;
                    break;
                    case 3:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T3BuyMult;
                    break;
                    case 4:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T4BuyMult;
                    break;
                    case 5:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T5BuyMult;
                    break;
                    case 6:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T6BuyMult;
                    break;
                    case 7:
                        item.AdjustedPrice = item.Cost * BaseBuyMult * T7BuyMult;
                    break;
                    }
                    CargoScreen.AddRow(new Markup($"Name: {item.Name}"), new Markup($"Cost: {item.AdjustedPrice}"));
                }
            }
            else{
                var tmp = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Nothing to sell, press any key to continue[/]")
                    .AllowEmpty());
                return;
            }

            Table ToSell = new Table();
            ToSell.AddColumns("ITEM","SELL PRICE");
            string SellOpts = "";
            foreach(Cargo item in inPlayer.PShip.CargoHold){
                if(StationBuyList.Contains(item.Type)){
                    SellOpts += item.Name + "|";
                    ToSell.AddRow($"{item.Name}", $"{item.AdjustedPrice}", $"{item.Count}");
                }
            }
            SellOpts += "Return";
            SellTable.AddRow(CargoScreen, ToSell);
            AnsiConsole.Write(SellTable);

            string[] options = SellOpts.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}

            string itemName = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an Item to Sell:")
            .PageSize(pageCount)
            .AddChoices(options));
            
            if(itemName == "Return"){
                return;
            }
            else{
                foreach(Cargo selling in inPlayer.PShip.CargoHold){ // Once you select the item to sell, push down into that menu
                    if(selling.Name == itemName){
                        inPlayer.SellItem(selling);
                        break;
                    }
                }
            }
        }

        public void UpgradeMenu(Player inPlayer){ // Selects an upgrade and passes it along to the the ship functions
            Table ShipInfo = new Table();
            ShipInfo.AddColumn("SHIP MANIFEST");
            ShipInfo.AddRow($"Chassis: {inPlayer.PShip.Chassis}");
            ShipInfo.AddRow($"Name: {inPlayer.PShip.Shield.Name} Cost: {inPlayer.PShip.Shield.Cost}");
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Armor.Name} Cost: {inPlayer.PShip.Armor.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Heatsink.Name} Cost: {inPlayer.PShip.Heatsink.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Engine.Name} Cost: {inPlayer.PShip.Engine.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Laser.Name} Cost: {inPlayer.PShip.Laser.Cost}"));
            ShipInfo.AddRow(new Markup($"Name: {inPlayer.PShip.Missile.Name} Cost: {inPlayer.PShip.Missile.Cost}"));

            AnsiConsole.Write(ShipInfo);
            string items = "";
            if(Type == "Military"){
                items = "Lasers|Missiles|Return";
            }
            else{
                items = "Chassis|Shield|Armor|Engine|Heatsink|Missiles|Lasers|Return";
            }
            string[] options = items.Split('|');
            int pageCount = options.Length + 1;
            if(pageCount <= 3){pageCount = 4;}
            string upgradeCat = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an sytem to upgrade:")
            .PageSize(6)
            .AddChoices(options));

            if(upgradeCat == "Return"){ return;}
            else{inPlayer.UpgradeSystem(upgradeCat);}
        }

        public void RestockMenu(Player inPlayer){
            inPlayer.RestockMissiles(5); // Allow restock for $5 per misssile
        }
        
        public void GoblinKing(){
            AnsiConsole.Clear();
            var image = new CanvasImage("//Data//david-bowie-labyrinth.jpg");
            var tmp = AnsiConsole.Prompt(
                    new TextPrompt<string>("")
                    .AllowEmpty());
        }
    }
}