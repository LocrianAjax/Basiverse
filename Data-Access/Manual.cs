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
                string[] temp =  System.IO.File.ReadAllLines(location);
                List<string> lines = new List<string>();
                foreach(string line in temp){ // Clean up the lines before splitting
                    if(line.Contains("//")){
                        continue;
                    }
                    else{
                        lines.Add(line);
                    }
                }

                // Now that we have the comments removed, we need to create the pages and split
                bool Titled = false;
                List<string> tempLines = new List<string>();
                string tempTitle = "";
                int Height = System.Console.WindowHeight - 10; // This gives us a buffer space around the text

                foreach(string line in lines){
                    if(line.Contains("::") && !Titled){
                        Title = line.Substring(2);
                        Titled = true;
                    } 

                    if(line.Contains("\\") || tempLines.Count > Height){ // If we hit the end of the page or hit the height limit, we need to cut a new one
                        Page tempPage = new Page(tempTitle, tempLines); // Shallow copy the list of lines
                        Pages.Add(tempPage);

                        tempLines.Clear(); // Then clear the temp lines
                    }
                    else if(line.Contains(":")){
                        tempTitle = line.Substring(1);
                    }
                    else{
                        tempLines.Add(line);
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