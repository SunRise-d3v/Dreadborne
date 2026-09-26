using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal sealed class Server : IDisposable
{
    private readonly IPAddress _anyIP = IPAddress.Any;
    private const ushort PORT = 7777;
    private bool _run;

    public void Start()
    {

        TcpListener server = new(_anyIP, PORT);
        server.Start();
        _run = true;

        Log(messageColor: [ConsoleColor.Green], messages: "The server is running and waiting for connections.");
        ushort clientID = 0;

        while (_run)
        {
            TcpClient clent = server.AcceptTcpClient();
            clientID++;

            NetworkStream stream = clent.GetStream();

            string message = $"[Player_ID:{clientID}]\n";
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            // Исправлено: все именованные аргументы идут после позиционных
            //Log(messages: $"Player_ID:{clientID}  ", "Hello from client!", messageColor: [ConsoleColor.White, ConsoleColor.Gray]);
            Log(messageColor: [ConsoleColor.White, ConsoleColor.Gray, ConsoleColor.Gray],
                messages: [$"Player_ID:{clientID}", "->", "Hello from client!"]);
            stream.Close();

            clent.Close();
        }

        server.Stop();
    }

    private static void Log(string warning = "Server", ConsoleColor warningColor = ConsoleColor.Green,
        ConsoleColor[] messageColor = null, params string[] messages)
    {
        const ConsoleColor defaultColor = ConsoleColor.White;

        // Server Log
        Console.Write('[');
        Console.ForegroundColor = warningColor;
        Console.Write(warning);
        Console.ForegroundColor = defaultColor;
        Console.Write(']');

        // Time
        Console.Write('[');
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(DateTime.Now.ToString("HH:mm:ss"));
        Console.ForegroundColor = defaultColor;
        Console.Write(']');

        // Message
        Console.Write('[');
        if (messages != null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                Console.ForegroundColor = (messageColor != null && i < messageColor.Length)
                    ? messageColor[i]
                    : defaultColor;

                Console.Write(messages[i]);
            }
        }
        Console.ForegroundColor = defaultColor;
        Console.Write(']');

        Console.Write('\n');
    }

    public void Dispose()
    {

    }
}