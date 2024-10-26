using ColdWar;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("   Welcome to Cold War: LAN Edition");
        Console.WriteLine("   Prepare for an intense experience!");
        Console.WriteLine("=========================================");

        // Create an instance of ReverseProtocol
        ReverseProtocol reverseProtocol = new ReverseProtocol();

        // Ask user to choose between server and client
        Console.WriteLine("Enter 'serve' to act as server or 'connect' to act as client:");
        string role = Console.ReadLine().Trim().ToLower();

        Country country = null;

        if (role == "serve")
        {
            // Start server
            reverseProtocol.Serve();
            country = SelectCountry("Server");
        }
        else if (role == "connect")
        {
            // Start client
            Console.Write("Enter the server IP address to connect: ");
            string serverIp = Console.ReadLine().Trim();
            reverseProtocol.Connect(serverIp);
            country = SelectCountry("Client");
        }
        else
        {
            Console.WriteLine("Invalid input. Exiting.");
            return; // Exit if input is invalid
        }

        // Display selected country info
        if (country != null)
        {
            Console.WriteLine($"Client chose: {country.Name}");
            Console.WriteLine("Press Enter to start the game...");
            Console.ReadLine(); // Wait for Enter to start the game
            PlayGame(country, reverseProtocol);
        }

        // Close the socket when done
        reverseProtocol.Close();
    }

    static Country SelectCountry(string role)
    {
        Console.WriteLine($"{role}, choose your side:");
        Console.WriteLine("1. USA (United States of America)");
        Console.WriteLine("2. USSR (Soviet Union)");

        while (true)
        {
            string choice = Console.ReadLine().Trim();

            if (choice == "1")
            {
                return new USA(300000000, new List<string> { "New York", "Los Angeles", "Chicago" }, 5000, "Ronald Reagan");
            }
            else if (choice == "2")
            {
                return new USSR(250000000, new List<string> { "Moscow", "Saint Petersburg", "Kyiv" }, 4000, "Mikhail Gorbachev");
            }
            else
            {
                Console.WriteLine("Invalid choice. Please choose 1 for USA or 2 for USSR.");
            }
        }
    }

    static void PlayGame(Country country, ReverseProtocol reverseProtocol)
    {
        int turn = 1;

        while (true)
        {
            Console.Clear(); // Clear the console for better readability
            Console.WriteLine($"Turn {turn}");
            Console.WriteLine($"{country.Name} starts");
            Console.WriteLine($"Current Leader: {country.CurrentLeader}");
            Console.WriteLine($"Current Income: {country.Income:C}");
            Console.WriteLine($"Population: {country.Population}");
            Console.WriteLine($"Warheads Capacity: {country.WarheadsCapacity}");
            Console.WriteLine("Choose your action:");
            Console.WriteLine("1. Wait");
            Console.WriteLine("2. Buy Warheads");
            Console.WriteLine("3. Attack");

            string action = Console.ReadLine().Trim();

            switch (action)
        {
            case "1":
                Console.WriteLine($"{country.Name} chose to wait.");
                break;
            case "2":
                Console.WriteLine($"{country.Name} chose to buy warheads.");
                // Implement buying logic here
                break;
            case "3":
                Console.WriteLine($"{country.Name} chose to attack.");
                // Ask the user for the number of warheads to launch
                Console.Write("Enter the number of warheads to launch: ");
                if (int.TryParse(Console.ReadLine(), out int warheadsToLaunch))
                {
                    // Assuming we have access to the opponent's country instance (you might need to adjust this part)
                    // Here we will need to send the opponent's country through the method
                    // This is a placeholder; you'd have to manage this based on your game state.
                    Country opponentCountry = (country.Name == "USA") ? new USSR(250000000, new List<string> { "Moscow", "Saint Petersburg", "Kyiv" }, 4000, "Mikhail Gorbachev")
                                                                     : new USA(300000000, new List<string> { "New York", "Los Angeles", "Chicago" }, 5000, "Ronald Reagan");

                    country.LaunchAttack(opponentCountry, warheadsToLaunch);
                }
                else
                {
                    Console.WriteLine("Invalid input for warheads.");
                }
                break;
            default:
                Console.WriteLine("Invalid action. Try again.");
                    continue; // Go back to the start of the loop without increasing turn
            }

            // If valid action is performed, increment turn
            turn++;
            Console.WriteLine("Press Enter to continue to the next turn...");
            Console.ReadLine(); // Wait for user to press Enter before the next turn
        }
    }
}
