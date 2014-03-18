using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using System.IO.Pipes;

namespace HippoKinectServer {
    static class HippoMain {       
        const int NO_CONNECTION = 0;
        const int ALIVE_CONNECTION = 1;


        static Queue action_list = new Queue();
        static int pipe_state;
        static int kinect_state;
        static Thread thread_DataSwapper;
        static Thread thread_KinnectControaller;

        static void Main() {
            thread_DataSwapper = new Thread(new ThreadStart(Thread_DataSwapper));
            thread_KinnectControaller = new Thread(new ThreadStart(Thread_KinnectControaller));
            thread_DataSwapper.Start();
            thread_KinnectControaller.Start();

            #region Test Code
            Thread thread_Test = new Thread(new ThreadStart(Thread_Test));
            thread_Test.Start();
            #endregion

            Thread_Hoster();
        }

        static void Thread_DataSwapper() {
            //Wait Thread Hoster init
            Thread.Sleep(2000);
            GlobalHelper.LOG("MESSAGE", "Thread DataSwapper Light up.");
            System.Threading.Interlocked.Exchange(ref pipe_state, NO_CONNECTION);

            MessageSender ms = new MessageSender();
            //Init
            ms.init(GlobalHelper.PIPE_NAME, GlobalHelper.SECRET_WORD);
            System.Threading.Interlocked.Exchange(ref pipe_state, ALIVE_CONNECTION);
            GlobalHelper.LOG("MESSAGE", "Pipe Service Startup Success.");

            #region Test Code
            //string kasi = ms.recive();
            //GlobalHelper.LOG("MESSAGE", kasi);
            ms.send("Hello World!");
            #endregion

        }

        static void Thread_KinnectControaller() {
            //Wait Thread Hoster init
            Thread.Sleep(2000);
            GlobalHelper.LOG("MESSAGE", "Thread KinnectControaller Light up.");
        }

        static void Thread_Hoster() {
            GlobalHelper.Init();

            lock (action_list) {
                action_list.Clear();
            }
            GlobalHelper.LOG("MESSAGE", "Thread Hoster Light up.");

            while (true) {
                //check log to write
                GlobalHelper.WriteLog();
                //
            }
        }

        #region Test Code
        static void Thread_Test() {
            Thread.Sleep(2000);
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(GlobalHelper.PIPE_NAME);

            pipeClient.Connect();
            StreamWriter sw = new StreamWriter(pipeClient);
            StreamReader sr = new StreamReader(pipeClient);
            sw.AutoFlush = true;

            while (true) {
                string message = sr.ReadLine();
                if (String.IsNullOrEmpty(message)) {

                } else {
                    GlobalHelper.LOG("MESSAGE", message);
                }
            }
        }
        #endregion
    }
}
