using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FRSystemProtecter
{
    class Program
    {
        static bool protecterState = false;
        static int checkDelayTime = 60000;
        static Process processFRSystem = null;


        static bool IsWorkTime()
        {
            bool doWork = true;

            int minHour = 6;
            int maxHour = 22;

            DateTime curTime = DateTime.Now;

            if (curTime.Hour < minHour || curTime.Hour > maxHour)
            {
                doWork = false;
            }

            return doWork;
        }

        static void StartFRSystem()
        {
            processFRSystem = new Process();
            processFRSystem.StartInfo.FileName = "南京图慧地铁站大流量实时动态人脸识别智能分析系统.exe";
            processFRSystem.Start();

            Console.WriteLine(DateTime.Now.ToString() + ":人脸识别智能分析系统已启动！");
        }

        static void StopFRSystem()
        {          
            processFRSystem.Kill();
            Console.WriteLine(DateTime.Now.ToString() + ":人脸识别智能分析系统已停止！");
        }


        static void CheckFRSystemState(object obj)
        {
            while (protecterState)
            {
                if(true == IsWorkTime())
                {
                    if (processFRSystem.HasExited)
                    {
                        StartFRSystem();
                    }
                }
                else
                {
                    if (false == processFRSystem.HasExited)
                    {
                        StopFRSystem();
                    }
                }

                Thread.Sleep(checkDelayTime);
            }

        }

        static void Main(string[] args)
        {
            protecterState = true;

            StartFRSystem();

            Thread checkThread = new Thread(new ParameterizedThreadStart(CheckFRSystemState));
            checkThread.IsBackground = true;
            checkThread.Start();

            Console.WriteLine(DateTime.Now.ToString() + ":人脸识别程序守护进程已启动！");

            string inputCmd = "";
            do
            {
                inputCmd = Console.ReadLine();
                inputCmd = inputCmd.ToUpper();

            } while (false == inputCmd.Equals("EXIT"));

            StopFRSystem();
        }
        
    }
}
