// 2014.03.06  Ramesh M Commented Gps state updation CR#61563
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics;

namespace DeliveryStreamCloudWinServ
{
    /// <summary>
    /// Partial DeliveryStreamCloudService1 class
    /// </summary>
    partial class DeliveryStreamCloudService1 : ServiceBase
    {
        private System.Timers.Timer tmrAutoLogout;
        // 2014.03.06  Ramesh M Commented Gps state updation CR#61563
        //private System.Timers.Timer tmrUpdateGPSHis;

        private System.Timers.Timer tmrCalcualateStatus;
        private bool inProcessUpdateState;
        private bool inProcessCalcStatus;
        private int running;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DeliveryStreamCloudService1()
        {
            tmrAutoLogout = new System.Timers.Timer(GetCofig.AutoLogOut_Interval() * 60000);
            // 2014.03.06  Ramesh M Commented Gps state updation CR#61563
            //tmrUpdateGPSHis = new System.Timers.Timer(GetCofig.UpdateState_Interval() * 60000);
            
            tmrCalcualateStatus = new System.Timers.Timer(GetCofig.CalcualateStatus_Interval() * 60000);

            tmrAutoLogout.Elapsed += new System.Timers.ElapsedEventHandler(tmrAutoLogout_Elapsed);
            // 2014.03.06  Ramesh M Commented Gps state updation CR#61563
           // tmrUpdateGPSHis.Elapsed += new System.Timers.ElapsedEventHandler(tmrUpdateGPSHis_Elapsed);

            tmrCalcualateStatus.Elapsed += new System.Timers.ElapsedEventHandler(tmrCalcualateStatus_Elapsed);
            InitializeComponent();
        }

        void tmrCalcualateStatus_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            CalcuateStatus();
        }
        // 2014.03.06  Ramesh M Commented Gps state updation CR#61563
        //void tmrUpdateGPSHis_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    updateState();
        //}

        //void updateState()
        //{
        //    if (!inProcessUpdateState)
        //    {
        //        inProcessUpdateState = true;
        //        Operation.UpdateGPSHistoryState();
        //        inProcessUpdateState = false;
        //    }
        //}


        void CalcuateStatus()
        {
            if (!inProcessCalcStatus)
            {
                inProcessCalcStatus = true;
                Operation.CalculateDriverStatus();
                inProcessCalcStatus = false;
            }
        }


        void tmrAutoLogout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Operation.AutoLogOut();
        }

        /// <summary>
        /// OnStart
        /// </summary>
        /// <param name="args">Scalar argument</param>
        protected override void OnStart(string[] args)
        {
            try
            {
               //Debugger.Launch();
                new Thread(new ThreadStart(ThreadProc)).Start();
                //runThread();
                tmrAutoLogout.Start();
               
                // 2014.03.06  Ramesh M Commented Gps state updation CR#61563
                //tmrUpdateGPSHis.Start();
                //inProcessUpdateState = false;

                inProcessCalcStatus = false;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop()
        {
            tmrAutoLogout.Stop();

            //tmrUpdateGPSHis.Stop();
        }

        /// <summary>
        /// ThreadProc
        /// </summary>
        public static void ThreadProc()
        {

            TimeSpan timeOutInt = TimeSpan.FromMinutes((double)Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]));
            while (true)
            {
                WebServices.CallWebService();
                //Thread.Sleep(timeOutInt);
            }
        }

        public void runThread()
        {
            if (Interlocked.CompareExchange(ref running, 1, 0) == 0)
            {
                Thread t = new Thread
                (
                    () =>
                    {
                        try
                        {
                            new Thread(new ThreadStart(ThreadProc)).Start();
                        }
                        catch
                        {
                            //Without the catch any exceptions will be unhandled
                            //(Maybe that's what you want, maybe not*)
                        }
                        finally
                        {
                            //Regardless of exceptions, we need this to happen:
                            running = 0;
                        }
                    }
                );
                t.IsBackground = true;
                t.Name = "myThread";
                t.Start();
            }
            else
            {
                Logging.WriteLog("Previous Process is already running...", EventLogEntryType.Error);
            }

        }

    }
}
