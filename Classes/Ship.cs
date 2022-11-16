using System;
using Basiverse;
using System.Collections.Generic;
using Spectre.Console;

namespace Basiverse
{
    [Serializable]
    class Ship
    {
        // A ship has a name, a type, a hull, a armor, a shield, a heat value, a heatsink, missiles, lasers, and a cargo hold
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private string _chassis = "";
        public string Chassis{ get {return _chassis;} set {_chassis = value;}}
        private int _heat = 0;
        private Hull _hull;
        public Hull Hull{ get {return _hull;} set {_hull = value;}}
        private Shield _shield;
        public Shield Shield{ get {return _shield;} set {_shield = value;}}
        private Armor _armor;
        public Armor Armor{ get {return _armor;} set {_armor = value;}}
        private Heatsink _heatsink;
        public Heatsink Heatsink{ get {return _heatsink;} set {_heatsink = value;}}
        private Missile _missiles;
        public Missile Missile{ get {return _missiles;} set {_missiles = value;}}
        private Laser _lasers;
        public Laser Laser{ get {return _lasers;} set {_lasers = value;}}
        private Engine _engine;
        public Engine Engine{ get {return _engine;} set {_engine = value;}}
        private CargoHold _hold;
        public CargoHold Hold{ get {return _hold;} set {_hold = value;}}

        public List<Cargo> CargoHold {get; set;}

        public Ship(){ // Set up the starter ship in the constructor
            _chassis = "Basicorps Runner"; // Set Default type
            _name = "The Nameless"; // Set Default Name

            _hull = new Hull("Basic Pressure Hull", 50, 25);  // Set Default Hull
            
            _armor = new Armor("None", 0, 0);

            _shield = new Shield("Basicorps Asteroid-B-Gone", 30, 0);

            _heatsink = new Heatsink("Basicorps therm01", 1, 10, 0);

            _lasers = new Laser("Basicorps Presentation Laser", 5, 2, 0);

            _missiles = new Missile("Basicorps Bottle Rockets", 10, .6, 0);
            _missiles.Stock = 10;
            
            _engine = new Engine("Basic Manuvering Thrusters", .3, 0);

            _hold = new CargoHold("Glovebox", 5);
            _hold.CurrentSize = 0;
            
            CargoHold = new List<Cargo>(); // Keep this seprate from the CargoHold obj to keep continuity when upgrading ships
        }

        // Overloaded constuctor that takes chassis
        public Ship(string inName, Chassis inChassis, Armor inArmor, Shield inShield, Heatsink inHeatsink, Laser inLaser, Missile inMissile, Engine inEngine){
            _name = inName;
            _hull = inChassis._Hull;
            _hold = inChassis._Cargohold;
            _chassis = inChassis.Name;
            _armor = inArmor;
            _shield = inShield;
            _heatsink = inHeatsink;
            _lasers = inLaser;
            _missiles = inMissile;
            _engine = inEngine;
        }

        // Overloaded constructor thaat takes Hull/Hold instead
        public Ship(string inName, string inChassis, Hull inHull, Armor inArmor, Shield inShield, Heatsink inHeatsink, Laser inLaser, Missile inMissile, Engine inEngine, CargoHold inHold){
            _name = inName;
            _chassis = inChassis;
            _hull = inHull;
            _armor = inArmor;
            _shield = inShield;
            _heatsink = inHeatsink;
            _lasers = inLaser;
            _missiles = inMissile;
            _engine = inEngine;
            _hold = inHold;
        }


        // Public Methods
        public void DisplayData(){ // Big data dump
            Console.WriteLine("\nBegin Ship Manifest:\n");
            Console.WriteLine("Name: {0}\nChassis: {1}\n", _name, _chassis);
            Console.WriteLine("Hull Report -\nName: {0}\nHealth: {1}%\nHeat Limit: {2}\n", _hull.Name, _hull.Health(), _hull.HeatMax);
            if(_shield.IsOnline){Console.WriteLine("Shields -\nName: {0}\nHealth: {1}%\nStatus: Online\n", _shield.Name, _shield.Health());}
            else{Console.WriteLine("Shields -\nName: {0}\nHealth: {1}%\nStatus: Offline\n", _shield.Name, _shield.Health());}
            Console.WriteLine("Heatsink -\nName: {0}\nCurrent Heat: {1}\n", _heatsink.Name, _heat);
            Console.WriteLine("Weapons Report -\n\nLasers:\nName: {0}\nDamage: {1}\nHeat Production: {2}\n\nMissiles:\nName: {3}\nHit Chance: {4}%\nStock: {5}\n", _lasers.Name, _lasers.Damage, _lasers.Heat, _missiles.Name, _missiles.HitChance * 100, _missiles.Stock);
            Console.WriteLine("Cargo Manifest - \nName: {0}\nSize: {1}m3\nAvailable Space: {2}m3\n", _hold.Name, _hold.MaxSize, (_hold.MaxSize - _hold.CurrentSize));
            Console.WriteLine("\nEnd Manifest");
        }

        public void Dashboard(){
            Console.WriteLine();
        }

        public int FireLaser(){ // Adds heat and returns damage
            _heat += _lasers.Heat; // Just increments heat since it's protected
            return _lasers.Damage; // Return the damage amount
        }

        public int FireMissile(){ // Returns damage if hit, 0 if miss
            if (_missiles.Stock == 0){
                return -1; // Error, out of stock
            }
            else{
                _missiles.Stock--;
                var rand = new Random();
                double randNumber = rand.NextDouble();
                if (randNumber <= _missiles.HitChance){ // If the number we generate between 0 and 1 is less than the hit chance we hit. IE .6 hits for a 70% chance
                    return _missiles.Damage; // Damage if hit, 0 if miss 
                }
                else{
                    return 0;
                }
            }
        }

        public bool Flee(){
             var rand = new Random();
            double randNumber = rand.NextDouble();
            if (randNumber <= _engine.FleeChance){ // If the number we generate between 0 and 1 is less than the hit chance we flee
                return true;
            }
            else{
                return false;
            }
        }

        public void CombatPassive(bool verbose){ // Calls the passive functions that happen each round.
            double res = CheckHeat();
            if((res == 1) && verbose){
                AnsiConsole.MarkupLine("[red]Warning! Excessive heat levels are damaging the hull![/]");
            }
            _shield.Regen();
        }

        public double CheckHeat(){ // Checks heat and deals damage to the hull
            if(_heatsink.IsActive && (_heat > _heatsink.ActiveVal)){
                _heat -= _heatsink.ActiveVal;
                _heatsink.IsActive = false; // Turn it back off it it's on
            }
            else if(!_heatsink.IsActive && (_heat > _heatsink.PassiveVal)){
                _heat -= _heatsink.PassiveVal;
            }

            if(_heat > _hull.HeatMax){
                double heatDamage = _heat - _hull.HeatMax; // Damage is equal to the damage amount over the hull's max
               _hull.Hullval -= heatDamage;
               return 1; // Return 1 for damage
            }
            return 0; // Return 0 for safe range
        }

        public double HullVal(){
            return Math.Floor(_hull.Health());
        }

        public double HeatVal(){
            return Math.Floor(HeatPercentage());
        }

        public double ShieldVal(){
            return Math.Floor(_shield.Health());
        }
    
        public void TakeDamage(int damage){ // Deals the damage to the ship's hull and/or shields
            if(_shield.IsOnline && (_shield.ShieldVal > 0)){ // Check if the shield is online
                double hullDamage = damage - _shield.ShieldVal; // Figure out hull damage
                _shield.ShieldVal -= damage; // Deal damage to the shield, then the hull
                if(hullDamage > 0){
                    HullDamage(hullDamage);
                }
                if(_shield.ShieldVal <= 0){ // Make sure it's always 0 if it goes negative and set it offline
                    _shield.ShieldVal = 0;
                    _shield.IsOnline = false;
                }
            }
            else{ // Otherwise just deal damage to the hull directly
                HullDamage(damage);
            }
        }

        public void RestartShields(bool verbose){ // Accesses the private vars and slightly recharges the shield
            _shield.IsOnline = true;
            _shield.ShieldVal = (int)Math.Floor(_shield.ShieldMax * .2); // Round it down to the nearest whole number
            if(verbose){
                AnsiConsole.WriteLine($"Shields Back Online at {_shield.Health()}%");
            }
        }

        public void RestoreShields(){ // Turns on and fully restores the shield
            _shield.IsOnline = true;
            _shield.ShieldVal = _shield.ShieldMax;
        }
    
        public void ActivateHeatsink(bool verbose){// Just accesses the private var
            _heatsink.IsActive = true;
            if(verbose){
                AnsiConsole.WriteLine($"Auxillary cooling systems engaged");
            }
        }
    
        public bool CheckDestroyed(){ // Check and see if the ship is destroyed
            if(_hull.Hullval <= 0){
                return true; // Return game over trigger
            }
            else{
                return false;
            }
        }

        public bool AddCargo(Cargo incoming, int amount){
            if(CheckCargo(incoming, amount)){ // Check size
                for(int i = 0; i < amount; i++){
                    _hold.CurrentSize = _hold.CurrentSize + incoming.Size;
                    if(CargoHold.Contains(incoming)){
                        CargoHold[CargoHold.IndexOf(incoming)].Count++;
                    }
                    else{
                        CargoHold.Add(incoming);
                    }
                }
                return true; // Otherwise return 1 for success
            }
            else{
                return false; // Return if it can't fit
            }
        }

        public void RemoveCargo(Cargo inCargo, int amount){ // This assumes that the amount you are removing exists
            for(int i = 0; i < amount; i++){
                if(CargoHold[CargoHold.IndexOf(inCargo)].Count > 1){
                    CargoHold[CargoHold.IndexOf(inCargo)].Count--;
                }
                else{
                    CargoHold.Remove(inCargo);
                }
                _hold.CurrentSize -= inCargo.Size;
            }
        }

        public bool CheckCargo(Cargo incoming, int amount){
            if((_hold.CurrentSize == _hold.MaxSize) || (((_hold.CurrentSize + incoming.Size) * amount) > _hold.MaxSize)){ // Check size
                return false; // Return false if it can't fit
            }
            else{
                return true; // Otherwise return true for a fit
            }
        }

        public void DumpCargo(){
            string options = "";
            foreach(Cargo item in CargoHold){
                 options += item.Name + "|";
            }
            options += "Return";
            string[] DumpOpts = options.Split('|');
            int length = DumpOpts.Length + 1;
            if(length < 3){ length = 3;}

            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Dump which item?")
                .PageSize(length)
                .AddChoices(DumpOpts));

            if(selection == "Return"){
                return;
            } 
            else{
                Cargo tempCargo = new Cargo();
                foreach(Cargo item in CargoHold){
                    if(item.Name == selection){
                        tempCargo = item;
                        break;
                    }
                }
                while(true){
                    AnsiConsole.WriteLine($"You have {tempCargo.Count} {tempCargo.Name} in the hold");
                    int dumpAmount = AnsiConsole.Ask<int>("How many would you like to dump?: ");
                    if(dumpAmount == 0){
                        return;
                    }
                    if(dumpAmount > tempCargo.Count){
                        AnsiConsole.WriteLine("Cannot dump more than you have");
                    }
                    if(dumpAmount < 0){
                        AnsiConsole.WriteLine("Cannot dump a negftive amount");
                    }
                    else{
                        RemoveCargo(tempCargo, dumpAmount);
                        return;
                    }
                }

            }
        }

        public bool Rename(string newName){
            if(String.IsNullOrEmpty(newName) || String.IsNullOrWhiteSpace(newName)){
                return false; // Return false if there's no name
            }
            else{
                _name = newName;
                return true;
            }
        }

        public bool Repair(int value){
            if((_hull.Hullval + value) > _hull.HullMax){
                return false;
            }
            else{
                _hull.Hullval += value;
                return true;
            }
        }
        
        public void UpgradeChassis(Chassis inChassis){
            Hull = inChassis._Hull;
            Hold = inChassis._Cargohold;
            _chassis = inChassis.Name;
        }
        
        // Private Methods
        private double HeatPercentage(){ // Returns the math on the heat to get the %
            return (_heat / _hull.HeatMax) * 100;
        }

        private void HullDamage(double damage){ // Checks armor and damages the hull
                double hullDamage = damage - _armor.ArmorValue;
                _hull.Hullval -= damage;
        }
    }
}