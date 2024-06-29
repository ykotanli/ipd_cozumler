using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

public class TcpServer
{
    private const int BufferSize = 1024;
    private const string LibraryFolder = "Libraries";

    public static void Main()
    {
        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8000);
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine("Server is listening on port 8000...");

            while (true)
            {
                using (Socket clientSocket = listener.Accept())
                {
                    Console.WriteLine("A client connected.");
                    byte[] buffer = new byte[BufferSize];
                    int bytesRead = clientSocket.Receive(buffer);
                    string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string[] tokens = request.Split(',');

                    if (tokens.Length == 3)
                    {
                        string dllName = tokens[0];
                        int arg1 = int.Parse(tokens[1]);
                        int arg2 = int.Parse(tokens[2]);

                        try
                        {
                            int result = LoadDllAndExecute(dllName, arg1, arg2);
                            byte[] msg = Encoding.ASCII.GetBytes(result.ToString());
                            clientSocket.Send(msg);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            byte[] msg = Encoding.ASCII.GetBytes("Error executing request.");
                            clientSocket.Send(msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid request format.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Server error: {e.ToString()}");
        }
    }

    private static int LoadDllAndExecute(string dllName, int arg1, int arg2)
    {
        string dllPath = Path.Combine(Directory.GetCurrentDirectory(), LibraryFolder, dllName);
        Assembly assembly = Assembly.LoadFrom(dllPath);
        Type type = assembly.GetType("ClassName"); // Yüklenen DLL'deki sınıfın adını belirtin.
        MethodInfo method = type.GetMethod("calculate");

        if (method == null)
        {
            throw new InvalidOperationException("Method 'calculate' not found in the DLL.");
        }

        object instance = Activator.CreateInstance(type);
        object[] parameters = new object[] { arg1, arg2 };
        int result = (int)method.Invoke(instance, parameters);
        return result;
    }
}
