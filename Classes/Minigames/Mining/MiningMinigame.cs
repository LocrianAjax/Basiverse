using System;
using Basiverse;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Basiverse
{

    class MiningMinigame{
        public Asteroid MiningAsteroid; // Using a default size of 6
        public int Size {get; set;}
    
        public MiningMinigame(){
            Size = 6;
            MiningAsteroid = new Asteroid(Size);
            MiningAsteroid.GenerateTiles();
        }

        public MiningMinigame(int inSize){
            Size = inSize;
            MiningAsteroid = new Asteroid(Size);
            MiningAsteroid.GenerateTiles();
        }

        public void StartMinigame(Player inPlayer){
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            // Draw a little fake Init screen
            var randProg = new Random();
            AnsiConsole.Progress().HideCompleted(true).Start(ctx => {
                var MiningInit = ctx.AddTask("[green]BRINGING MINING SUB-SYSTEMS ONLINE.......[/]");
                while(!MiningInit.IsFinished){
                    MiningInit.Increment(randProg.Next(1,10));
                    Thread.Sleep(75);
                }
                var LaserInit = ctx.AddTask("[green]TUNING LASERS TO MINING FREQUENCIES.......[/]");
                while(MiningInit.IsFinished && !LaserInit.IsFinished){
                    LaserInit.Increment(randProg.Next(1,10));
                    Thread.Sleep(75);
                }
            });
            // Set up the inital screen
            Thread.Sleep(300);
            AnsiConsole.Clear();
            DrawMiningAssist();
            AnsiConsole.Cursor.Hide();
            int selected = 0;
            MiningAsteroid.Tiles[selected].isSelected = true;
            MiningAsteroid.DrawAll();
            AnsiConsole.Cursor.Hide();

            //  Now that we're set up, start the loop
            while(true){
                DrawMiningAssist();
                ConsoleKey input = TimedReader.ReadKey(250);
                int oldSelected = selected;
                if(input == ConsoleKey.DownArrow){
                    if((selected + Size) > (Size * Size) - 1) {} // Do nothing
                    else{selected += Size;}
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.UpArrow){
                    if((selected - Size) < 0) {} // Do nothing
                    else{selected -= Size;}
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.LeftArrow){       
                    selected -= 1;
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.RightArrow){
                    selected += 1;
                    selected = ValidateAndDraw(selected, oldSelected);
                }
                if(input == ConsoleKey.Spacebar){
                    // Damage the tile and change it's version
                    HightlightAndDamage(selected);
                }

                // Check for game over case
                if(MiningAsteroid.CurrHealth <= 0){
                    DrawMiningAssist();
                    AnsiConsole.Cursor.SetPosition(0,3);
                    AnsiConsole.Write("Asteroid disintegration!");
                    EndMinigame(inPlayer);
                    return;
                }
            }
        }

        public void HightlightAndDamage(int inSelection){
            // Change to a highlight
            MiningAsteroid.Tiles[inSelection].isSelected = false;
            MiningAsteroid.Tiles[inSelection].isHighlighted = true;
            MiningAsteroid.DrawAt(inSelection);

            Thread.Sleep(100); // Pause for flair

            // Then Un-hilight and damage
            MiningAsteroid.Tiles[inSelection].Damage(); // TODO: Check for loot and add something for that here.
            MiningAsteroid.Tiles[inSelection].isHighlighted = false;
            MiningAsteroid.Tiles[inSelection].isSelected = true;
            MiningAsteroid.DrawAt(inSelection);
            MiningAsteroid.CurrHealth -= 5;
        }

        public int ValidateAndDraw(int inSelection, int oldSelection){
            MiningAsteroid.Tiles[oldSelection].isSelected = false;
            MiningAsteroid.DrawAt(oldSelection);
            int selected = inSelection;
            // Validate selection
            if(selected > (Size * Size) - 1){
                selected = (Size * Size) - 1;
            }
            else if(selected < 0){
                selected = 0;
            }

            // Then draw
            MiningAsteroid.Tiles[selected].isSelected = true;
            MiningAsteroid.DrawAt(selected);
            AnsiConsole.Cursor.Hide();
            return selected;
        }
    
        public void DrawMiningAssist(){
            AnsiConsole.Cursor.Hide();
            // Header in the middle
            string HeaderMsg = "MINING PROTOCOLS ACTIVE - TRACKING TARGET INTEGRITY";
            AnsiConsole.Cursor.SetPosition((Console.WindowWidth / 2) - (HeaderMsg.Length / 2),0);
            AnsiConsole.Markup($"[green]{HeaderMsg}[/]");

            // Then draw heat display info
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row 
            AnsiConsole.Write("                                         ");
            AnsiConsole.Cursor.SetPosition(0,1); // Reset to the row
            AnsiConsole.Markup($"[green]ASTEROID STRUCTURAL INTEGRITY[/]");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row
            AnsiConsole.Write("                                         ");
            AnsiConsole.Cursor.SetPosition(0,2); // Reset to the row

            // Draw the "Health Indicator"
            int RedSquares = (MiningAsteroid.MaxHealth / 4) - (MiningAsteroid.CurrHealth / 4);
            int GreenSquares = (MiningAsteroid.MaxHealth / 4) - RedSquares;
            for(int i = 0; i < GreenSquares; i++){// Use ▀ char in green and red to show the heat
                AnsiConsole.Markup($"[green]▀[/]");
            }
            for(int j = 0; j < RedSquares; j++){
                AnsiConsole.Markup($"[red]▀[/]");
            }
        }

        public void EndMinigame(Player inPlayer){
            int LootCount = 0;
            // Figure out how many tiles contain loot
            foreach(Tile tile in MiningAsteroid.Tiles){
                if(tile.WasLooted()){
                    LootCount++;
                }
            }
            
            if(LootCount == 0){ // Check for 0 loot
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[red]ASTEROID INTEGRITY LOST - NOTHING RECOVERABLE[/]");
                var tmp = AnsiConsole.Prompt(
                new TextPrompt<string>("Press Enter to continue.....")
                .AllowEmpty());
                return;
            }
            else{ // Otherwise we have loot
                List<Cargo> temp = BinarySerialization.ReadFromBinaryFile<List<Cargo>>("Data//cargo.bin");
                int GemsIndex = 0;
                int OresIndex = 0;
                int MineralsIndex = 0;
                int GritIndex = 0;

                int count = 0;
                foreach(Cargo cargo in temp){ // Find the Loot
                    if(cargo.Name == "Raw Mined Gems"){
                        GemsIndex = count;
                    }
                    else if(cargo.Name == "Mined Ores"){
                        OresIndex = count;
                    }
                    else if(cargo.Name == "Mined Minerals"){
                        MineralsIndex = count;
                    }
                    else if(cargo.Name == "Asteroid Grit"){
                        GritIndex = count;
                    }
                    count++;
                }

                List<Cargo> lootList = new List<Cargo>();
                var rand = new Random();
                for(int i = 0; i < LootCount; i++){ // Set up loot pool
                    int roll = rand.Next(0, 101);
                    if(roll >= 90){ // Gems
                        if(lootList.Contains(temp[GemsIndex])){
                            lootList[lootList.IndexOf(temp[GemsIndex])].Count++;
                        }
                        else{
                            lootList.Add(temp[GemsIndex]);
                        }
                    }
                    else if(roll >= 75 && roll < 90){ // Ores
                        if(lootList.Contains(temp[OresIndex])){
                            lootList[lootList.IndexOf(temp[OresIndex])].Count++;
                        }
                        else{
                            lootList.Add(temp[GemsIndex]);
                        }
                    }
                    else if(roll >= 50 && roll < 75){ // Minerals
                        if(lootList.Contains(temp[MineralsIndex])){
                            lootList[lootList.IndexOf(temp[MineralsIndex])].Count++;
                        }
                        else{
                            lootList.Add(temp[MineralsIndex]);
                        }
                    }
                    else{ // Grit
                        if(lootList.Contains(temp[GritIndex])){
                            lootList[lootList.IndexOf(temp[GritIndex])].Count++;
                        }
                        else{
                            lootList.Add(temp[GritIndex]);
                        }
                    }
                }
               
                
                while(true){
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[green]ASTEROID INTEGRITY LOST - RECOVERABLE: [/]");

                    foreach(Cargo item in lootList){
                        AnsiConsole.WriteLine($"{item.Name}: {item.Description} VALUE: {item.Cost} SIZE: {item.Size}m^3 (x){item.Count}");
                    }
                    AnsiConsole.WriteLine($"AVAILABLE CARGO SPACE: {inPlayer.PShip.Hold.Available()}m^3");

                    string innerOpts = "";
                    foreach(Cargo item in lootList){
                        innerOpts += $"{item.Name}|";
                    }
                    innerOpts += "Dump|Exit";
                    string[] options = innerOpts.Split("|");

                    string selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Pick up Loot?")
                    .PageSize(options.Length + 1)
                    .AddChoices(options));
                    
                    if(selection == "Dump"){
                        inPlayer.PShip.DumpCargo();
                    }
                    else if(selection == "Exit"){
                        return;
                    }
                    else{
                        Cargo tempCargo = new Cargo();
                        foreach(Cargo item in lootList){
                            if(item.Name == selection){
                                tempCargo = item;
                                break;
                            }
                        }
                        if(inPlayer.PShip.CheckCargo(lootList[lootList.IndexOf(tempCargo)], 1)){
                            inPlayer.PShip.AddCargo(tempCargo, 1);
                            if(lootList[lootList.IndexOf(tempCargo)].Count > 1){
                                lootList[lootList.IndexOf(tempCargo)].Count--;
                            }
                            else{
                                lootList.Remove(tempCargo);
                            }
                        }
                        else{
                            AnsiConsole.WriteLine("[red]Does not fit! Please free up some space[/]");
                        }
                    }
                }
            }
        }
    }
}