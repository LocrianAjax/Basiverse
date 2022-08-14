using System;
using Basiverse;

namespace Basiverse
{
    [Serializable]
    class Ship
    {
        // A ship has a name, a type, a hull, a armor, a shield, a heat value, a heatsink, missiles, lasers, and a cargo hold
        private string _name = "";
        public string Name{ get {return _name;} set {_name = value;}}
        private string _type = "";
        public string Type{ get {return _type;} set {_type = value;}}
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

        public Ship(){ // Set up the starter ship in the constructor
            _type = "Basicorps Runner"; // Set Default type
            _name = "The Nameless"; // Set Default Name

            _hull = new Hull();  // Set Default Hull
            _hull.Name = "Basic Pressure Hull";
            _hull.HeatMax = 25;
            _hull.HullMax = 50;
            _hull.Hullval = _hull.HullMax;
            
            _armor = new Armor();

            _shield = new Shield();
            _shield.Name = "Basicorps Asteroid-B-Gone"; // Set Default Shield
            _shield.ShieldMax = 30;
            _shield.ShieldVal = _shield.ShieldMax;

            _heatsink = new Heatsink();
            _heatsink.Name = "Basicorps therm01"; // Set Default Heatsink
            _heatsink.PassiveVal = 1;
            _heatsink.ActiveVal = 10;

            _lasers = new Laser();
            _lasers.Name = "Basicorps Presentation Laser"; // Set Default Laser
            _lasers.Damage = 5;
            _lasers.Heat = 2;

            _missiles = new Missile();
            _missiles.Name = "Basicorps Bottle Rockets"; // Set Default Missile
            _missiles.Damage = 10;
            _missiles.HitChance = .6;
            _missiles.Stock = 10;
            
            _engine = new Engine();
            _engine.Name = "Basic Manuvering Thrusters"; // Set Default Engine
            _engine.FleeChance = .3;

            _hold = new CargoHold();
            _hold.Name = "Glovebox"; // Set default cargo hold
            _hold.MaxSize = 5;
            _hold.CurrentSize = 0;
            _hold.HoldItems = null;
        }


        // Public Methods
        public void DisplayData(){ // Big data dump
            Console.WriteLine("\nBegin Ship Manifest:\n");
            Console.WriteLine("Name: {0}\nClass: {1}\n", _name, _type);
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

        public int CheckHeat(){ // Checks heat and deals damage to the hull
            if(_heatsink.IsActive){
                _heat -= _heatsink.ActiveVal;
                _heatsink.IsActive = false; // Turn it back off it it's on
                Console.WriteLine("Auxillary Cooling Pumps disengaged");
            }
            else{
                _heat -= _heatsink.PassiveVal;
            }

            if(_heat > _hull.HeatMax){
                Console.WriteLine("Warning! Excessive heat levels are damaging the hull!");
                int heatDamage = _heat - _hull.HeatMax; // Damage is equal to the damage amount over the hull's max
                return heatDamage; // Return the amount of damage done to the hull
            }
            return 0; // Return 0 for safe range
        }

        public double HullVal(){
            return _hull.Health();
        }

        public double HeatVal(){
            return HeatPercentage();
        }

        public double ShieldVal(){
            return _shield.Health();
        }
    
        public void TakeDamage(int damage){ // Deals the damage to the ship's hull and/or shields
            if(_shield.IsOnline && (_shield.ShieldVal > 0)){ // Check if the shield is online
                int hullDamage = damage - _shield.ShieldVal; // Figure out hull damage
                _shield.ShieldVal -= damage; // Deal damage to the shield, then the hull
                HullDamage(hullDamage);
                if(_shield.ShieldVal <= 0){ // Make sure it's always 0 if it goes negative and set it offline
                    _shield.ShieldVal = 0;
                    _shield.IsOnline = false;
                }
            }
            else{ // Otherwise just deal damage to the hull directly
                HullDamage(damage);
            }
        }

        public void RestartShields(){ // Accesses the private vars and slightly recharges the shield
            _shield.IsOnline = true;
            _shield.ShieldVal = (int)Math.Floor(_shield.ShieldMax * .2); // Round it down to the nearest whole number
            Console.WriteLine("Shields Back Online at {0}%", _shield.Health());
        }
    
        public void ActivateHeatsink(){// Just accesses the private var
            _heatsink.IsActive = true;
            Console.WriteLine("Heatsink Activated - Activating auxillary cooling pumps");
        }
    
        public bool CheckDestroyed(){ // Check and see if the ship is destroyed
            if(_hull.Hullval <= 0){
                return true; // Return game over trigger
            }
            else{
                return false;
            }
        }

        public int AddCargo(Cargo incoming){
            if((_hold.CurrentSize == _hold.MaxSize) || ((_hold.CurrentSize + incoming.Size) >= _hold.MaxSize)){ // Check size
                return -1; // Return if it can't fit
            }
            else{
                _hold.CurrentSize = _hold.CurrentSize + incoming.Size;
                _hold.HoldItems.Add(new Cargo() {Name = incoming.Name, Size = incoming.Size, Cost = incoming.Cost });
                return 1; // Otherwise return 1 for success
            }
        }

        public void RemoveCargo(int index){
            _hold.CurrentSize = _hold.CurrentSize - _hold.HoldItems[3].Size;
            _hold.HoldItems.RemoveAt(index);
        }

        // Private Methods
        private double HeatPercentage(){ // Returns the math on the heat to get the %
            return (_heat / _hull.HeatMax) * 100;
        }

        private void HullDamage(int damage){ // Checks armor and damages the hull
            if(_armor.Name != "none"){
                int hullDamage = damage - _armor.ArmorValue;
                _hull.Hullval -= hullDamage;
            }
            else{
                _hull.Hullval -= damage;
            }
        }
    }
}