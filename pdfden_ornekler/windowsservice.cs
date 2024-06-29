public partial class Service1 : ServiceBase
{
    private System.Timers.Timer timer;
    private int eventID;
    public Service1()
    {
        InitializeComponent();
        //this.ServiceName = "KTUN LOG Service; 
        this.EventLog.Log = "Application";
        //this.CanHandlePowerEvent = true;
        //this.CanHandleSessionChangeEvent = true;
        this.CanPauseAndContinue = true;
        this.CanShutdown = true;
        this.CanStop = true;
        //Timer
        eventID = 1;
        timer = new System.Timers.Timer();
        timer.Interval = 30000; // 60 seconds
        timer.Elapsed += new
       System.Timers.ElapsedEventHandler(this.OnTimer);
    }
    public void OnTimer(object sender,
   System.Timers.ElapsedEventArgs args)
    {
        this.EventLog.WriteEntry("Monitör sistemimdir kasim abi",
       EventLogEntryType.Information, eventID++);
    }
    protected override void OnStart(string[] args)
    {
        timer.Start();
        this.EventLog.WriteEntry("in onstart.starttayiz.");
    }
    protected override void OnStop()
    {
        timer.Stop();
        this.EventLog.WriteEntry("in onStop.stoptayiz.");
    }
    protected override void OnContinue()
    {
        timer.Start();
        this.EventLog.WriteEntry("Devam ediyoruz.");
    }
    protected override void OnPause()
    {
        timer.Stop();

        this.EventLog.WriteEntry("Pausedeyiz şu anda.");
    }
}