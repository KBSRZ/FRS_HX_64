using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace NjustSkyEyeSystem
{
    static class Program
    {

        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new FormMian());
        //}

        public delegate Int32 CallBack(ref long a);
        [DllImport("kernel32")]
        private static extern Int32 SetUnhandledExceptionFilter(CallBack cb);

        public static Int32 newexceptionfilter(ref long a)
        {
            LogHelper.WriteErrorLog("SetUnhandledExceptionFilter Work!");

            return 1;
        }

        public static CallBack myCallBack;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                myCallBack = new CallBack(newexceptionfilter);
                SetUnhandledExceptionFilter(myCallBack);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                FormMian mainForm = new FormMian();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("Application Run Error", ex);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.WriteErrorLog("Application_ThreadException", e.Exception);
            //Thread.Sleep(5000);
            //Application.Restart();
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            LogHelper.WriteErrorLog("CurrentDomain_UnhandledException", ex);
        }

    }
}
