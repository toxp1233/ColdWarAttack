using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using ColdWar;

public class ReverseProtocol
{
    private Socket _socket;
    private const int Port = 12345;
    private string _playerSide;
    private Country _country;
    private Socket _clientSocket;
    private const int IncomeIncrease = 3000000;

    public void Serve()
    {
        ListAvailableIPs();

        Console.Write("Enter your IP address to listen on: ");
        string ipAddress = Console.ReadLine();

        IPAddress ip = IPAddress.Parse(ipAddress);
        _socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(new IPEndPoint(ip, Port));
        _socket.Listen(1);

        Console.WriteLine($"Server started, listening on {ipAddress}:{Port}...");

        _clientSocket = _socket.Accept();
        Console.WriteLine("Client connected.");

        HandleClient(_clientSocket);
    }

    public void Connect(string serverIp)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(serverIp, Port);
        Console.WriteLine("Connected to server.");

        HandleServer(_socket);
    }

    private void HandleClient(Socket clientSocket)
    {
        SendMessage(clientSocket, "Hello from the server! You are now part of the Cold War!");
        AssignSide(clientSocket, "Server");
        WaitForEnter();
        StartGame(clientSocket);
    }

    private void HandleServer(Socket serverSocket)
    {
        string welcomeMessage = "Hello from the client! Welcome to the Cold War!";
        SendMessage(serverSocket, welcomeMessage);
        AssignSide(serverSocket, "Client");
        WaitForEnter();
        StartGame(serverSocket);
    }

    private void AssignSide(Socket socket, string role)
    {
        string[] sides = { "USA", "USSR" };

        if (role == "Server")
        {
            _playerSide = sides[1];
            _country = new USSR(250000000, new List<string> { "Moscow", "Saint Petersburg", "Kyiv" }, 4000, "Mikhail Gorbachev");
            SendMessage(socket, $"You have been assigned: {sides[1]}");
            Console.WriteLine($"Server chose: {sides[1]}");
        }
        else
        {
            Console.WriteLine("Choose your side:");
            Console.WriteLine("1. USA (United States of America)");
            Console.WriteLine("2. USSR (Soviet Union)");
            string choice = Console.ReadLine();

            if (choice.Trim() == "1")
            {
                _playerSide = sides[0];
                _country = new USA(300000000, new List<string> { "New York", "Los Angeles", "Chicago" }, 5000, "Ronald Reagan");
                SendMessage(socket, sides[0]);
                Console.WriteLine($"Client chose: {sides[0]}");
            }
            else if (choice.Trim() == "2")
            {
                _playerSide = sides[1];
                _country = new USSR(250000000, new List<string> { "Moscow", "Saint Petersburg", "Kyiv" }, 4000, "Mikhail Gorbachev");
                SendMessage(socket, sides[1]);
                Console.WriteLine($"Client chose: {sides[1]}");
            }
            else
            {
                Console.WriteLine("Invalid choice. Assigning default side: USA.");
                _playerSide = sides[0];
                _country = new USA(300000000, new List<string> { "New York", "Los Angeles", "Chicago" }, 5000, "Ronald Reagan");
                SendMessage(socket, sides[0]);
                Console.WriteLine($"Client chose: {sides[0]}");
            }
        }
    }

    private void WaitForEnter()
    {
        Console.WriteLine("Press Enter to start the game...");
        Console.ReadLine();
    }

    private void StartGame(Socket socket)
    {
        Console.WriteLine("Turn 1");
        Console.WriteLine($"{_playerSide} starts");

        // Increase income at the beginning of the turn
        _country.Income += IncomeIncrease;

        // Show updated stats after income increase
        ShowCountryStats(socket);

        string gameState = "Choose your action:\n1. Wait\n2. Buy Warheads\n3. Attack";
        SendMessage(socket, gameState);
        Console.WriteLine(gameState);

        // Get the player's action
        string action = ReceiveMessage(socket);
        HandleAction(socket, action);
    }

    private void HandleAction(Socket socket, string action)
    {
        switch (action)
        {
            case "1":
                Console.WriteLine($"{_playerSide} chose to wait.");
                break;
            case "2":
                Console.WriteLine($"{_playerSide} chose to buy warheads.");
                // Implement buying logic here
                break;
            case "3":
                Console.WriteLine($"{_playerSide} chose to attack.");
                // Ask the user for the number of warheads to launch
                Console.Write("Enter the number of warheads to launch: ");
                if (int.TryParse(Console.ReadLine(), out int warheadsToLaunch))
                {
                    // Assuming we have access to the opponent's country instance (you might need to adjust this part)
                    // Here we will need to send the opponent's country through the method
                    // This is a placeholder; you'd have to manage this based on your game state.
                    Country opponentCountry = (_playerSide == "USA") ? new USSR(250000000, new List<string> { "Moscow", "Saint Petersburg", "Kyiv" }, 4000, "Mikhail Gorbachev")
                                                                     : new USA(300000000, new List<string> { "New York", "Los Angeles", "Chicago" }, 5000, "Ronald Reagan");

                    _country.LaunchAttack(opponentCountry, warheadsToLaunch);
                }
                else
                {
                    Console.WriteLine("Invalid input for warheads.");
                }
                break;
            default:
                Console.WriteLine("Invalid action. Try again.");
                break;
        }

        // Continue game logic, possibly for the next turn
        // You can also manage turns and player actions here
        ShowCountryStats(socket);
    }

    private void ShowCountryStats(Socket socket)
    {
        string stats = $"{_country.Name} Stats:\n" +
                       $"Population: {_country.Population}\n" +
                       $"Income: {_country.Income:C}\n" +
                       $"Warheads Capacity: {_country.WarheadsCapacity}\n";
        SendMessage(socket, stats);
        Console.WriteLine(stats);
    }

    private void SendMessage(Socket socket, string message)
    {
        byte[] msg = Encoding.ASCII.GetBytes(message);
        socket.Send(msg);
    }

    private string ReceiveMessage(Socket socket)
    {
        byte[] buffer = new byte[1024];
        int bytesReceived = socket.Receive(buffer);
        return Encoding.ASCII.GetString(buffer, 0, bytesReceived);
    }

    private void ListAvailableIPs()
    {
        Console.WriteLine("Available IPs:");
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            Console.WriteLine(ip);
        }
    }

    public void Close()
    {
        _socket?.Close();
    }
}

