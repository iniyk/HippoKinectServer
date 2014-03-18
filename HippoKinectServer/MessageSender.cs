using System;
using System.IO;
using System.IO.Pipes;

namespace HippoKinectServer {
    public class MessageSender {
        private NamedPipeServerStream pipeServer;
        private StreamReader pr;
        private StreamWriter pw;
        public MessageSender() {
            
        }
        public void init(string pipe_name, string secret_word) {
            pipeServer =
            new NamedPipeServerStream(pipe_name);
            pipeServer.WaitForConnection();

            try {
                pr = new StreamReader(pipeServer);
                pw = new StreamWriter(pipeServer);
                pw.AutoFlush = true;
            }
            catch (IOException e) {
                GlobalHelper.LOG("ERROR", "Catch a IO Exception on Message sender Initialization.");
            }
            GlobalHelper.LOG("MESSAGE", "Message sender Initialization has been Initializated successfully.");
        }
        public void send(string str) {
            try {
                pw.WriteLine(str);
                pw.Flush();
            } catch (IOException e) {
                GlobalHelper.LOG("ERROR", "Catch a IO Exception on Message sender sending message.");
            }
            
        }
        public string recive() {
            try {
                if (pr.Peek() == -1) {
                    GlobalHelper.LOG("MESSAGE", "No string to read.");
                    return null;
                }
                string ret = pr.ReadLine();
                if (String.IsNullOrEmpty(ret)) {
                    return null;
                }
                return ret;
            } catch (IOException e) {
                GlobalHelper.LOG("ERROR", "Catch a IO Exception on Message sender reciving message.");
                return null;
            }
        }
        public void kill() {
            pipeServer.Disconnect();
            GlobalHelper.LOG("MESSAGE", "Message sender Initialization has been Disconnected successfully.");
        }
    }
}
