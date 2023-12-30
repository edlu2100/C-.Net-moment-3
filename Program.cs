using System;
using System.Collections.Generic;
using System.IO;

using System.Text.Json;
namespace GuestBook
{
    public class GuestBook
    {
        private string filename = @"GuestBook.json"; // Filnamn för att spara data
        private List<Post> posts = new List<Post>(); // Lista för att lagra gästboksinlägg

        public GuestBook()
        {
            // Kontrollera om 'GuestBook.json' finns
            if(File.Exists(@"GuestBook.json")==true)
            {
                // Läs JSON-innehållet från filen och deserialisera det till listan med inlägg
                string? jsonString = File.ReadAllText(filename);
                posts = JsonSerializer.Deserialize<List<Post>>(jsonString);
            }
        }
        // Metod för att lägga till ett nytt inlägg i gästboken
        public Post addPost(Post post){
            posts.Add(post); // Lägg till inlägget i listan
            marshal(); //Spara den uppdaterade listan till filen
            return post;
        }

        // Metod för att ta bort ett inlägg 
        public int delPost(int index)
        {
            posts.RemoveAt(index); // Ta bort inlägget från listan
            marshal(); // Spara den uppdaterade listan till filen
            return index;
        }

        // Metod för att hämta alla posts
        public List<Post> getPosts()
        {
            return posts; // Returnera listan med inlägg
        }

        // Privat metod för att serialisera och spara listan med inlägg till filen
        private void marshal()
        {
            // Serialisera alla objekt och spara till fil
            var jsonString = JsonSerializer.Serialize(posts);
            File.WriteAllText(filename, jsonString);
        }

    }

    // Inlägg i gästboken
    public class Post
    {
        private string? content; // Innehållet i inlägget
        private string? author; // Innehållet i inlägget

        // Egenskap för att komma åt och ställa in innehållet
        public string? Content
        {
            set {this.content = value;}
            get {return this.content;}
        }
             public string? Author
        {
            set {this.author = value;}
            get {return this.author;}
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            GuestBook guestBook = new GuestBook(); // Skapa en instans av klassen GuestBook
            int i=0;

            while(true)
            {
                Console.Clear();Console.CursorVisible = false;
                //Skriver ut beskrivningar till consolen
                Console.WriteLine("Gästboken\n\n");
                Console.WriteLine("1. Skriv i Gästboken");
                Console.WriteLine("2. Ta bort inlägg\n");
                Console.WriteLine("X. Avsluta\n");

                i=0;

                // Loopa igenom och visa alla inlägg i gästboken
                foreach(Post post in guestBook.getPosts())
                {
                    //Skriver ut namn + innehåll i listan§
                    Console.WriteLine("[" + i++ + "] " + post.Author + " - " + post.Content);
                    
                }
                int inp = (int) Console.ReadKey(true).Key;

                //Vad som ska hända beroende på knapptryckning
                switch (inp){
                    case '1':
                        Console.CursorVisible = true;
                        Console.Write("Namn: ");
                        string? author = Console.ReadLine();
                        Post obj = new Post();

                        // Om det finns text lägg till i gästboken
                        while (String.IsNullOrEmpty(author))
                        {
                            Console.Write("Du måste fylla i ett namn: ");
                            author = Console.ReadLine();
                        }

                        Console.Write("Skriv inlägg ");
                        string? content = Console.ReadLine();
                        // Om det finns text lägg till i gästboken
                        while (String.IsNullOrEmpty(content))
                        {
                            Console.Write("Du måste fylla i innehåll: ");
                            content = Console.ReadLine();
                        }

                        // Lägg till Author och Content i obj
                        obj.Author = author;
                        obj.Content = content;

                        // Lägg till inlägget i gästboken
                        guestBook.addPost(obj);
                        break;
                    case '2':
                        Console.CursorVisible = true;
                        Console.Write("Ange index att radera: ");
                        string? index = Console.ReadLine(); //Läs index som är angivet av användaren
                        int delNr = Convert.ToInt32(index);
                        if (delNr >= 0 && delNr < guestBook.getPosts().Count)
                        {
                            // Visa innehållet för att varna användaren
                            Post postToDelete = guestBook.getPosts()[delNr];
                            Console.WriteLine($"Vill du radera följande :\n{postToDelete.Author} - {postToDelete.Content}");
                            Console.Write("Vill du fortsätta? (J/N): ");
                            ConsoleKeyInfo key = Console.ReadKey();

                            if (key.Key == ConsoleKey.J)
                            {
                                guestBook.delPost(delNr); // Radera inlägget från gästboken baserat på det angivna indexet
                                Console.WriteLine("\nInlägget har raderats.");
                            }
                            else
                            {
                                Console.WriteLine("\nRadering avbruten.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Index finns inte, testa ett nytt");
                        }
                        break;
                    case 88: 
                        Environment.Exit(0); // Avsluta programmet om användaren väljer alternativet 'X'
                        break;
                }
            }
        }
    }
}
