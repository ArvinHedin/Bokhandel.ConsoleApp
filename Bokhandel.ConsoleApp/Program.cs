using Bokhandel.ConsoleApp.Models;

using var context = new BookStoreContext();

bool running = true;
while (running)
{
    Console.WriteLine("1. Lista lagersaldo per butik");
    Console.WriteLine("2. Lägg till bok i butik");
    Console.WriteLine("3. Ta bort bok från butik");
    Console.WriteLine("4. Hantera böcker (CRUD)");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    switch (Console.ReadLine())
    {
        case "1":
            // Visa lagersaldo
            break;
        case "2":
            // Lägg till bok
            break;
        case "3":
            // Ta bort bok
            break;
        case "4":
            // CRUD för bok
            break;
        case "5":
            running = false;
            break;
    }
}
