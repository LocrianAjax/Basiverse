using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;

namespace Basiverse
{

    [Serializable]
    class Combat{ // A combat contains an NPC and then classes that take in a player
        public NPC NPC;
    
        public Combat(int difficulty){ // Generates a new combat with the specified difficulty
            var rand = new Random();
            Generator CombatGen = new Generator();
            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data\\combatnpcnames.data");
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0) + 1))], difficulty);
        }

        public void Fight(Player inPlayer){
            // Simmilar setup to the station start/loop
            while(true){
                // Do stuff
                AnsiConsole.Clear();
                CombatScreen(inPlayer);
                CombatMenu();
                inPlayer.PShip.CheckHeat();
                NPC.cShip.CheckHeat();
                if(inPlayer.PShip.CheckDestroyed()){
                    // GameOver();
                }
                else if(NPC.cShip.CheckDestroyed()){
                    // Victory();
                }
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
            double enemyShieldSw = Math.Floor(inPlayer.PShip.ShieldVal());
            if( enemyShieldSw >= 75){
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [green]{enemyShieldSw}[/]%"));
            }
            else if(enemyShieldSw < 75 && enemyShieldSw > 25 ){
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [yellow]{enemyShieldSw}[/]%"));
            }
            else if(enemyShieldSw == 0){
                EnemyReport.AddRow(new Markup($"Shields [red]]OFFLINE![/]"));
            }
            else{
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [darkorange]{enemyShieldSw}[/]%"));
            }

            // Enemy Hull Status
            double enemyHullSw = Math.Floor(inPlayer.PShip.HullVal());
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

        public void CombatMenu(){
            // TODO: This
            var tmp = AnsiConsole.Prompt(new TextPrompt<string>("Press any key to return").AllowEmpty());
        }

    }

}