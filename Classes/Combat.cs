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
        private string[] DisplayShipLines = {
        "   __",
        "  |  \\",
        "=}    `--._____",
        "=}   ,---------'-",
        "  |_/"
        };

        private string[] DispayShipLinesWithShield = {
        "[cyan]         , ~ ~ ~ ,[/]",
        "[cyan]     , '           ' ,[/]",
        "[cyan]   ,                   ,[/]",
        "[cyan]  ,[/]       __             [cyan],[/]",
        "[cyan] ,[/]       |  \\             [cyan],[/]",
        "[cyan] ,[/]      =}    `--._____    [cyan],[/]",
        "[cyan] ,[/]      =}  ,---------'-   [cyan],[/]",
        "[cyan]  ,[/]      |_/              [cyan],[/]",
        "[cyan]   ,                     ,[/]",
        "[cyan]     ,                 ,'[/]",
        "[cyan]       ' - , _ _ _  , '[/]"    
        };
    
        private string[] DisplayShipLinesOpp = {
        "            __",
        "           /  |",
        "  _____.--`   {=",
        "-'---------,  {=",
        "            \\_|"
        };

        private string[] DispayShipLinesWithShieldOpp = {
        "[cyan]         , ~ ~ ~ ,[/]",
        "[cyan]     , '           ' ,[/]",
        "[cyan]   ,                   ,[/]",
        "[cyan]  ,[/]             __       [cyan],[/]",
        "[cyan] ,[/]             /  |        [cyan],[/]",
        "[cyan] ,[/]    _____.--`    {=      [cyan],[/]",
        "[cyan] ,[/]  -'---------,   {=      [cyan],[/]",
        "[cyan]  ,[/]             \\_|       [cyan],[/]",
        "[cyan]   ,                     ,[/]",
        "[cyan]     ,                 ,'[/]",
        "[cyan]       ' - , _ _ _  , '[/]"    
        };

        public Combat(int difficulty, int NPCAttitude){ // Generates a new combat with the specified difficulty and a specific attitude
            var rand = new Random();
            Generator CombatGen = new Generator();
            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data//combatnpcnames.data");
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0)))], difficulty, NPCAttitude);
        }

        public Combat(int difficulty){ // Generates a new combat with the specified difficulty and a random attitude
            var rand = new Random();
            Generator CombatGen = new Generator();
            string [] NameLines;
            NameLines = System.IO.File.ReadAllLines("Data//combatnpcnames.data");
            int NPCAttitude = rand.Next(0, 5);
            NPC = CombatGen.GenerateCombatNPC(NameLines[rand.Next(0, (NameLines.GetLength(0)))], difficulty, NPCAttitude);
        }

        public int Fight(Player inPlayer){
            while(true){ // Loop until a ship is destroyed or flees
                AnsiConsole.Clear();
                CombatScreen(inPlayer);
                int res = CombatMenu(inPlayer); // Menu for combat options
                int npcRes = NPCTurn(inPlayer); // NPC takes it's turn
                inPlayer.PShip.CombatPassive(true); // Do the passive stuff that happens each turn
                NPC.cShip.CombatPassive(false);

                if(res == 1){ // Check if either of us are fleeing
                    var fleeTmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty()); // Pause before returning
                    inPlayer.logFlee(NPC.cShip.Name, true);
                    return 0;
                }
                else if(npcRes == 1){
                    var fleeTmp = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty()); // Pause before returning
                    inPlayer.logFlee(NPC.cShip.Name, false);
                    return 1;
                }

                if(inPlayer.PShip.CheckDestroyed()){ // Check if we're dead
                    return -1;
                }
                else if(NPC.cShip.CheckDestroyed()){ // Check if we've won
                    inPlayer.logVictory(NPC.cShip.Name);
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
            ShipReport.AddColumns($"THE {inPlayer.PShip.Name} STATUS");
            // Add rows for Hull/Heat/Shield

            // Shield Status
            if(inPlayer.PShip.Shield.IsOnline){
                ShipReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [{inPlayer.PShip.Shield.GetShieldColor()}]{inPlayer.PShip.ShieldVal()}[/]%"));
            }
            else{
                ShipReport.AddRow(new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }

            // Hull Status
            ShipReport.AddRow(new Markup($"Hull Integrity: [{inPlayer.PShip.Hull.GetHullColor()}]{inPlayer.PShip.HullVal()}[/]%"));

            // Heat Status
            ShipReport.AddRow(new Markup($"{inPlayer.PShip.Heatsink.GetOnlineStr()} Heat Soak - [{inPlayer.PShip.Heatsink.GetHeatColor(inPlayer.PShip.HeatVal())}]{inPlayer.PShip.HeatVal()}[/]%"));

            // Weapon Status
            ShipReport.AddRow($"Lasers Damage {inPlayer.PShip.Laser.Damage} Heat Generation {inPlayer.PShip.Laser.Heat}");
            ShipReport.AddRow($"Missiles Damage {inPlayer.PShip.Missile.Damage} Hit Chance {inPlayer.PShip.Missile.HitChance * 100}% Stock {inPlayer.PShip.Missile.Stock}");
            
            // Draw the player ship
            if(inPlayer.PShip.Shield.IsOnline){
                foreach(string line in DispayShipLinesWithShield){
                    ShipReport.AddRow(line);
                }
            }
            else{
                foreach(string line in DisplayShipLines){
                    ShipReport.AddRow(line);
                }
            }
            

            // Enemy data
            Table EnemyReport = new Table();
            EnemyReport.AddColumn($"ENEMY DATA: BROADCAST NAME THE {NPC.cShip.Name}");

            // Enemy Shield Status
            if(inPlayer.PShip.Shield.IsOnline){
                EnemyReport.AddRow(new Markup($"Shields [cyan]ONLINE[/] - Strength: [{NPC.cShip.Shield.GetShieldColor()}]{NPC.cShip.ShieldVal()}[/]%"));
            }
            else{
                EnemyReport.AddRow(new Markup($"Shields [red][slowblink]OFFLINE[/][/]"));
            }

            // Enemy Hull Status 
            EnemyReport.AddRow(new Markup($"Hull Integrity: [{NPC.cShip.Hull.GetHullColor()}]{NPC.cShip.HullVal()}[/]%"));
            
            // Draw the enemy ship
            if(NPC.cShip.Shield.IsOnline){
                foreach(string line in DispayShipLinesWithShieldOpp){
                    EnemyReport.AddRow(line);
                }
            }
            else{
                foreach(string line in DisplayShipLinesOpp){
                    EnemyReport.AddRow(line);
                }
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
            if(inPlayer.PShip.Shield.IsOnline){
                opts += "Overload Shields|";
            }
            if(inPlayer.PShip.Heatsink.Name != "None"){
                opts += "Activate Heatsink|";
            }
            opts += "Flee";

            string[] options = opts.Split('|');
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Options:")
                .PageSize(options.Length + 3)
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
                    inPlayer.PShip.RestartShields(true);
                    AnsiConsole.Write(new Markup($"[green]Shields Online[/]\n"));
                break;
                case "Overload Shields":
                    inPlayer.PShip.OverloadShield(true);
                    AnsiConsole.Write(new Markup($"[yellow]System Overloaded![/]\n"));
                break;
                case "Activate Heatsink":
                    inPlayer.PShip.ActivateHeatsink(true);
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
                        AnsiConsole.Write($"Enemy Ship The {NPC.cShip.Name} has fled!\n");
                        return 1;
                    }
                    else{
                        AnsiConsole.Write($"Enemy Ship The {NPC.cShip.Name} has attemped to flee, but they couldn't get away!\n");
                        return 0;
                    }
                case 1:
                    NPC.cShip.ActivateHeatsink(false);
                    AnsiConsole.Write($"Enemy Ship The {NPC.cShip.Name} activated it's auxillary cooling pumps\n");
                break;
                case 2:
                    int laserDam = NPC.cShip.FireLaser();
                    inPlayer.PShip.TakeDamage(laserDam);
                    AnsiConsole.Write(new Markup($"[red]DAMAGE REPORT![/] Sustained {laserDam} damage!\n"));
                break;
                case 3:
                    int missilesDam = NPC.cShip.FireMissile();
                    AnsiConsole.Write($"Enemy Ship The {NPC.cShip.Name} launched missiles!\n");
                    AnsiConsole.Progress().HideCompleted(false).Start(ctx => {
                            var evade = ctx.AddTask("[red]Attempting evasive manuevers![/]");
                            var rand = new Random();
                            while(!evade.IsFinished){
                                evade.Increment(rand.Next(0, 25));
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
                    NPC.cShip.RestartShields(false);
                    AnsiConsole.Write($"Enemy Ship the {NPC.cShip.Name} is restarting it's shield!\n");
                break;
            }
            return 0;
        }
    
    
    }

}