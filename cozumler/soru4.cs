using System;
using System.ServiceProcess;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public partial class EchoService : ServiceBase
{
    private Socket listenerSocket;
    private bool isRunning = true;

    public EchoService()
    {
        InitializeComponent();
        this.CanStop = true;
        this.CanPauseAndContinue = true;
    }

    protected override void OnStart(string[] args)
    {
        Thread serviceThread = new Thread(StartListening);
        serviceThread.Start();
    }

    private void StartListening()
    {
        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8000);

        listenerSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listenerSocket.Bind(localEndPoint);
            listenerSocket.Listen(10);

            while (isRunning)
            {
                Socket clientSocket = listenerSocket.Accept();
                byte[] buffer = new byte[1024];
                int bytesRead = clientSocket.Receive(buffer);
                clientSocket.Send(buffer, bytesRead, SocketFlags.None);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        catch (Exception ex)
        {
            this.EventLog.WriteEntry($"Error: {ex.Message}");
        }
    }

    protected override void OnStop()
    {
        isRunning = false;
        listenerSocket.Close();
    }
}
