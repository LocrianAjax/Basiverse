using System.Collections.Generic;
using Spectre.Console;
using System.IO;
using System;

namespace Basiverse{

    public class Page{
        public string Title { get; set; }
        public List<string> lines;

        public Page(){
            lines = new List<string>();
            Title = "";
        }

        public Page(List<string> CloneList){
            lines = new List<string>(CloneList);
        }

        public Page(string inTitle, List<string> CloneList){
            lines = new List<string>(CloneList);
            Title = inTitle;
        }
    }

    public class Manual{
        List<Page> Pages;
    
        public string Title { get; set; }

        public int PageIndex { get; set; }

        public Manual(){
            Pages = new List<Page>();
            Title = "";
        }

        public Manual(List<Page> ClonePage){
            Pages = new List<Page>(ClonePage);
            Title = "";
        }

        public Manual(string inTitle, List<Page> ClonePage){
            Pages = new List<Page>(ClonePage);
            Title = inTitle;
        }

        public void Load(string location){
            if (File.Exists(location)){
                int Height = System.Console.WindowHeight - 10; // This gives us a buffer space around the text
                string[] temp =  System.IO.File.ReadAllLines(location);
                Page tempPage = new Page();
                foreach(string line in temp){ // Clean up the comments
                    if(line.Contains("//")){
                        continue;
                    }
                    else{
                        if(line.Contains("::") && (Title == "")){ // Add a title to the manual if none exists
                            Title = line.Substring(2);
                        } 
                        else if(line.Contains("\\") || tempPage.lines.Count > Height){ // If we hit the end of the page or hit the height limit, we need to cut a new one
                            Page inPage = new Page(tempPage.Title, tempPage.lines);
                            Pages.Add(inPage);
                            tempPage.lines.Clear(); // Then clear the temp lines
                            tempPage.Title = "";
                        }
                        else if(line.Contains(":")){ // Set the page title
                            tempPage.Title = line.Substring(1);
                        }
                        else{
                            tempPage.lines.Add(line);
                        }
                    }
                }
            }
            else{
                AnsiConsole.WriteLine("File not found: " + location);
                return;
            }
        }

        public void Display(){ // Figures out how many lines we can display, and handles showing them
            PageIndex = 0;
            while(PageIndex < Pages.Count){
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText(Title));
                AnsiConsole.WriteLine(Pages[PageIndex].Title);
                
                foreach(string line in Pages[PageIndex].lines){
                    AnsiConsole.WriteLine(line);
                }
                AnsiConsole.Cursor.SetPosition(0, Console.WindowHeight);
                AnsiConsole.Write($"Page {PageIndex + 1} of {Pages.Count}");

                ConsoleKey input = Console.ReadKey().Key;
                if(input == ConsoleKey.RightArrow || input == ConsoleKey.DownArrow){
                    PageIndex++;
                }
                else if(input == ConsoleKey.LeftArrow || input == ConsoleKey.UpArrow){
                    if(PageIndex > 0){
                        PageIndex--;
                    }
                }
            }
        }
    }
}