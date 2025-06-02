using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bokhandel.ConsoleApp.Models;

var context = new BookStoreContext();
bool running = true;

string[] menuItems = new[]
{
    "Visa lagersaldo per butik",
    "Lägg till bok i butik",
    "Ta bort bok från butik",
    "Avsluta"
};

while (running)
{
    Console.CursorVisible = false;
    int selectedIndex = DisplayMenu(menuItems);
    Console.Clear();
    

    switch (selectedIndex)
    {
        case 0:
            ListInventory(context);
            break;
        case 1:
            AddBookToStore(context);
            break;
        case 2:
            RemoveBookFromStore(context);
            break;
        case 3:
            running = false;
            break;
    }

    if (running)
    {
        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
        Console.ReadKey();
    }
}

int DisplayMenu(string[] items)
{
    int selectedIndex = 0;
    ConsoleKey key;

    do
    {
        Console.Clear();
        Console.WriteLine("==== BOKHANDEL ====\n");

        for (int i = 0; i < items.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(items[i]);
            Console.ResetColor();
        }

        key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                selectedIndex = (selectedIndex == 0) ? items.Length - 1 : selectedIndex - 1;
                break;
            case ConsoleKey.DownArrow:
                selectedIndex = (selectedIndex + 1) % items.Length;
                break;
        }
    } while (key != ConsoleKey.Enter);

    return selectedIndex;
}

void ListInventory(BookStoreContext context)
{
    var stores = context.Stores
        .Include(s => s.InverntoryBalances)
        .ThenInclude(i => i.IsbnNavigation)
        .ToList();

    foreach (var store in stores)
    {
        Console.WriteLine($"\nButik: {store.Name}");
        foreach (var item in store.InverntoryBalances)
        {
            Console.WriteLine($"- {item.IsbnNavigation.Title} (Antal: {item.Amount})");
        }
    }
}

void AddBookToStore(BookStoreContext context)
{
    var books = context.Books.ToList();
    var stores = context.Stores.ToList();

    Console.WriteLine("\nVälj en bok:");
    var bookIndex = DisplayMenu(books.Select(b => $"{b.Isbn}: {b.Title}").ToArray());
    string isbn = books[bookIndex].Isbn;

    Console.WriteLine("\nVälj en butik:");
    var storeIndex = DisplayMenu(stores.Select(s => $"{s.Id}: {s.Name}").ToArray());
    int storeId = stores[storeIndex].Id;

    Console.Write("\nAnge antal: ");
    int amount = int.Parse(Console.ReadLine());

    var existing = context.InverntoryBalances
        .FirstOrDefault(i => i.Isbn == isbn && i.StoreId == storeId);

    if (existing != null)
    {
        existing.Amount = (existing.Amount ?? 0) + amount;
        Console.WriteLine("Uppdaterade existerande saldo.");
    }
    else
    {
        var newEntry = new InverntoryBalance
        {
            Isbn = isbn,
            StoreId = storeId,
            Amount = amount
        };
        context.InverntoryBalances.Add(newEntry);
        Console.WriteLine("Bok lades till i butik.");
    }

    context.SaveChanges();
}

void RemoveBookFromStore(BookStoreContext context)
{
    var stores = context.Stores.Include(s => s.InverntoryBalances).ThenInclude(i => i.IsbnNavigation).ToList();

    Console.WriteLine("\nVälj en butik:");
    var storeIndex = DisplayMenu(stores.Select(s => $"{s.Id}: {s.Name}").ToArray());
    var store = stores[storeIndex];

    if (!store.InverntoryBalances.Any())
    {
        Console.WriteLine("Butiken har inga böcker i lager.");
        return;
    }

    Console.WriteLine("\nVälj en bok att ta bort:");
    var inventoryIndex = DisplayMenu(store.InverntoryBalances.Select(i => $"{i.Isbn}: {i.IsbnNavigation.Title}").ToArray());
    var selectedBook = store.InverntoryBalances.ElementAt(inventoryIndex);

    context.InverntoryBalances.Remove(selectedBook);
    context.SaveChanges();
    Console.WriteLine("Bok togs bort från butik.");
}
