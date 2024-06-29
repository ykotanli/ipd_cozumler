using System;
using System.Net.Sockets;
using System.Text;

public class TcpClientExample
{
    public static void Main()
    {
        string server = "127.0.0.1";  // Sunucunun IP adresi
        int port = 8000;             // Sunucunun dinlediği port

        using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            client.Connect(server, port);
            Console.WriteLine("Connected to the server.");

            string dllName = "MyMathLibrary.dll";
            int arg1 = 5;
            int arg2 = 3;
            string request = $"{dllName},{arg1},{arg2}";
            byte[] message = Encoding.ASCII.GetBytes(request);

            client.Send(message);

            byte[] buffer = new byte[1024];
            int received = client.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, received);
            Console.WriteLine($"Server response: {response}");

            client.Shutdown(SocketShutdown.Both);
        }
    }
}
