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
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0)))], difficulty);
        }

        public int Fight(Player inPlayer){
            while(true){ // Loop until a ship is destroyed or flees
                AnsiConsole.Clear();
                CombatScreen(inPlayer);
                int res = CombatMenu(inPlayer); // Menu for combat options
                int npcRes = NPCTurn(inPlayer); // NPC takes it's turn
                inPlayer.PShip.CombatPassive(); // Do the passive stuff that happens each turn
                NPC.cShip.CombatPassive();

                if(res == 1){ // Check if either of us are fleeing
                    var fleeTmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty()); // Pause before returning
                    return 0;
                }
                else if(npcRes == 1){
                    var fleeTmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty()); // Pause before returning
                    return 1;
                }

                if(inPlayer.PShip.CheckDestroyed()){ // Check if we're dead
                    return -1;
                }
                else if(NPC.cShip.CheckDestroyed()){ // Check if we've won
                    return 2;
                }
                var tmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty()); // Pause before next turn
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
            double heatSw = Math.Floor(inPlayer.PShip.HeatVal());
            if(heatSw <= 25){
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [green]{heatSw}[/]%"));
            }
            else if(heatSw <= 75 && heatSw > 25){
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [yellow]{heatSw}[/]%"));
            }
            else{
                ShipReport.AddRow(new Markup($"{OnlineStat} Heat Soak - [red]{heatSw}[/]%"));
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
                    AnsiConsole.Write(new Markup($"[green]Hit[/] for {laserDam} damage!\n"));
                break;
                case "Fire Missiles":
                    int missilesDam = inPlayer.PShip.FireMissile();
                    if(missilesDam == 0){
                        AnsiConsole.Write(new Markup("[red]Missed![/]\n"));
                    }
                    else{
                        AnsiConsole.Write(new Markup($"[green]Hit[/] for {missilesDam} damage!\n"));
                        NPC.cShip.TakeDamage(missilesDam);
                    }
                break;
                case "Restart Shields":
                    inPlayer.PShip.RestartShields();
                    AnsiConsole.Write(new Markup($"[green]Shields Online[/]\n"));
                break;
                case "Activate Heatsink":
                    inPlayer.PShip.ActivateHeatsink();
                    AnsiConsole.Write(new Markup($"[green]Heatsink Online[/]\n"));
                break;
                case "Flee":
                    if(inPlayer.PShip.Flee()){
                        AnsiConsole.Write(new Markup($"[green]You got away![/]\n"));
                        return 1;
                    }
                    else{
                        AnsiConsole.Write(new Markup($"[red]Couldn't get away![/]\n"));
                    }
                break;
            }

            return 0;
        }

        public int NPCTurn(Player inPlayer){
            int choice = NPC.Decide();
            // 0 for flee, 1 for activate heatsink, 2 for laser, 3 for missiles, 4 for restart shields
            switch(choice){
                case 0:
                    if(NPC.cShip.Flee()){
                        AnsiConsole.Write($"Enemy Ship {NPC.cShip.Name} has fled!\n");
                        return 1;
                    }
                    else{
                        AnsiConsole.Write($"Enemy Ship {NPC.cShip.Name} has attemped to flee, but they couldn't get away!\n");
                        return 0;
                    }
                case 1:
                    NPC.cShip.ActivateHeatsink();
                    AnsiConsole.Write($"Enemy Ship {NPC.cShip.Name} activated it's auxillary cooling pumps\n");
                break;
                case 2:
                    int laserDam = NPC.cShip.FireLaser();
                    inPlayer.PShip.TakeDamage(laserDam);
                    AnsiConsole.Write(new Markup($"[red]DAMAGE REPORT![/] Sustained {laserDam} damage!\n"));
                break;
                case 3:
                    int missilesDam = NPC.cShip.FireMissile();
                    AnsiConsole.Write($"Enemy Ship {NPC.cShip.Name} launched missiles!\n");
                    AnsiConsole.Progress().HideCompleted(false).Start(ctx => {
                            var evade = ctx.AddTask("[red]Attempting evasive manuevers![/]");
                            while(!evade.IsFinished){
                                evade.Increment(10);
                                Thread.Sleep(100);
                            }
                        });
                    if(missilesDam == 0){
                        AnsiConsole.Write(new Markup($"Evasive manuvers [green]succeded![/]\n"));
                    }
                    else{
                        AnsiConsole.Write(new Markup($"Evasive manuvers [red]failed![/]\n"));
                        AnsiConsole.Write(new Markup($"[red]DAMAGE REPORT![/] Sustained {missilesDam} damage!\n"));
                        inPlayer.PShip.TakeDamage(missilesDam);
                    }
                break;
                case 4:
                    NPC.cShip.RestartShields();
                    AnsiConsole.Write($"Enemy Ship {NPC.cShip.Name} is restarting it's shield!\n");
                break;
            }
            return 0;
        }
    
    
    }

}