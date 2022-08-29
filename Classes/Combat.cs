using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{

    [Serializable]
    class Combat{ // A combat contains an NPC and then classes that take in a player
        public NPC NPC;
    
        public Combat(int difficulty){ // Generates a new combat with the specified difficulty
            var rand = new Random();
            Generator CombatGen = new Generator();
            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data//combatnpcnames.data");
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0) + 1))], difficulty);
        }

        public void Fight(Player inPlayer){
            // Simmilar setup to the station start/loop
            while(true){
                AnsiConsole.Clear();
                CombatScreen(inPlayer);
                int res = CombatMenu(inPlayer);
                inPlayer.PShip.CheckHeat();
                NPC.cShip.CheckHeat();
                if(inPlayer.PShip.CheckDestroyed()){
                    // GameOver();
                }
                else if(NPC.cShip.CheckDestroyed()){
                    // Victory();
                    AnsiConsole.WriteLine("A Winner is you!");
                }
                else if(res == 1){
                    return;
                }
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
            }
        }

        public void CombatScreen(Player inPlayer){
            // Create main table
            Table MainTable = new Table();

            // Add data for player ship
            Table ShipReport = new Table();
            ShipReport.AddColumns($"{inPlayer.PShip.Name} STATUS");
            // Add rows for Hull/Heat/Shield
            // Shield Status
            double shieldSw = Math.Floor(inPlayer.PShip.ShieldVal());
            if( shieldSw >= 75){
                ShipReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [green]{shieldSw}[/]%"));
            }
            else if(shieldSw < 75 && shieldSw > 25 ){
                ShipReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [yellow]{shieldSw}[/]%"));
            }
            else if(shieldSw == 0){
                ShipReport.AddRow(new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }
            else{
                ShipReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [darkorange]{shieldSw}[/]%"));
            }

            // Hull Status
            double hullSw = Math.Floor(inPlayer.PShip.HullVal());
            if(hullSw >= 75){
                ShipReport.AddRow(new Markup($"Hull Integrity: [green]{hullSw}[/]%"));
            }
            else if (hullSw <= 25){
                ShipReport.AddRow(new Markup($"Hull Integrity: [red]{hullSw}[/]%"));
            }
            else{
                ShipReport.AddRow(new Markup($"Hull Integrity: [yellow]{hullSw}[/]%"));
            }

            // Heat Status
            string OnlineStat;
            if(inPlayer.PShip.Heatsink.IsActive){
                OnlineStat = "[green]Active Cooling[/]";
            }
            else{
                OnlineStat = "[red]Passive Cooling[/]";
            }
            if(inPlayer.PShip.HeatVal() <= 25){
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [green]{inPlayer.PShip.HeatVal()}[/]%"));
            }
            else if(inPlayer.PShip.HeatVal() <= 75 && inPlayer.PShip.HeatVal() > 25){
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [yellow]{inPlayer.PShip.HeatVal()}[/]%"));
            }
            else{
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [red]{inPlayer.PShip.HeatVal()}[/]%"));
            }

            // Weapon Status
            ShipReport.AddRow($"Lasers Damage {inPlayer.PShip.Laser.Damage} Heat Generation {inPlayer.PShip.Laser.Heat}");
            ShipReport.AddRow($"Missiles Damage {inPlayer.PShip.Missile.Damage} Hit Chance {inPlayer.PShip.Missile.HitChance * 100}% Stock {inPlayer.PShip.Missile.Stock}");
            

            // Enemy data
            Table EnemyReport = new Table();
            EnemyReport.AddColumn($"ENEMY DATA: BROADCAST NAME {NPC.cShip.Name}");
            // Enemy Shield Status
            double enemyShieldSw = Math.Floor(NPC.cShip.ShieldVal());
            if( enemyShieldSw >= 75){
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [green]{enemyShieldSw}[/]%"));
            }
            else if(enemyShieldSw < 75 && enemyShieldSw > 25 ){
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [yellow]{enemyShieldSw}[/]%"));
            }
            else if(enemyShieldSw == 0){
                EnemyReport.AddRow(new Markup($"Shields [red]OFFLINE![/]"));
            }
            else{
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [darkorange]{enemyShieldSw}[/]%"));
            }

            // Enemy Hull Status
            double enemyHullSw = Math.Floor(NPC.cShip.HullVal());
            if(enemyHullSw >= 75){
                EnemyReport.AddRow(new Markup($"Hull Integrity: [green]{enemyHullSw}[/]%"));
            }
            else if (enemyHullSw <= 25){
                EnemyReport.AddRow(new Markup($"Hull Integrity: [red]{enemyHullSw}[/]%"));
            }
            else{
                EnemyReport.AddRow(new Markup($"Hull Integrity: [yellow]{enemyHullSw}[/]%"));
            }

            MainTable.AddColumns("SHIP DATA", "COMBAT SENSORS");
            MainTable.AddRow(ShipReport, EnemyReport);
            MainTable.Expand();
            AnsiConsole.Write(MainTable);
        }

        public int CombatMenu(Player inPlayer){
            string opts = "";
            //  Generate options string dynamically
            if(inPlayer.PShip.Laser != null){
                opts += "Fire Lasers|";
            }
            if(inPlayer.PShip.Missile.Stock > 0){
                opts += "Fire Missiles|";
            }
            if(!inPlayer.PShip.Shield.IsOnline){
                opts += "Restart Shields|";
            }
            if(inPlayer.PShip.Heatsink.Name != "None"){
                opts += "Activate Heatsink|";
            }
            opts += "Flee";

            string[] options = opts.Split('|');
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Options:")
                .PageSize(5)
                .AddChoices(options));

            switch(selection){
                case "Fire Lasers":
                    int laserDam = inPlayer.PShip.FireLaser();
                    NPC.cShip.TakeDamage(laserDam);
                    AnsiConsole.Write(new Markup($"[green]Hit[/] for {laserDam} damage!"));
                break;
                case "Fire Missiles":
                    int missilesDam = inPlayer.PShip.FireMissile();
                    if(missilesDam == 0){
                        AnsiConsole.Write(new Markup("[red]Missed![/]"));
                    }
                    else{
                        AnsiConsole.Write(new Markup($"[green]Hit[/] for {missilesDam} damage!"));
                        NPC.cShip.TakeDamage(missilesDam);
                    }
                break;
                case "Restart Shields":
                    inPlayer.PShip.RestartShields();
                    AnsiConsole.Write(new Markup($"[green]Shields Online[/]"));
                break;
                case "Activate Heatsink":
                    inPlayer.PShip.ActivateHeatsink();
                    AnsiConsole.Write(new Markup($"[green]Heatsink Online[/]"));
                break;
                case "Flee":
                    if(inPlayer.PShip.Flee()){
                        AnsiConsole.Write(new Markup($"[green]You got away![/]"));
                        return 1;
                    }
                    else{
                        AnsiConsole.Write(new Markup($"[red]Couldn't get away![/]"));
                    }
                break;
            }

            return 0;
        }

        public void NPCTurn(Player inPlayer){
            // If missiles > 0 shoot missiles or laser 50% chance
            // If missiles = 0 shoot laser until heat is too high
            // If heat is > 75% 50% chance to shoot or activate heatsink
            // If heat is > 95% activate heatsink
            // If hull is < 75% 20% chance to flee, increase chance by 5% for each 5% hull decrease

            if(NPC.cShip.HullVal() < 75){
                // Check for flee if not, fall down to the attack section
            }

            else if(NPC.cShip.Missile.Stock > 0){
                
            }
            else if(NPC.cShip.HeatVal() > 75){
                if(NPC.cShip.HeatVal() > 95){

                }
                else{

                }
            }

        }
    
    
    }

}