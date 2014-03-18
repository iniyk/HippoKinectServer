using System;
using System.IO;
using System.Collections;

namespace HippoKinectServer {
    public class GlobalHelper {
        public const string SECRET_WORD = "uIIde34ewD";
        public const string PIPE_NAME = "HippoKinectServiceTransPipe";
        public const int MAX_THREAD = 4;
        public const string LOG_FILE_PATH = @"E:\log.txt";

        public static Queue log_list;

        public GlobalHelper() {
        }
        private static void AddLog(string type, string log) {
            StreamWriter sw;
            if (File.Exists(LOG_FILE_PATH)) {
                sw = File.AppendText(LOG_FILE_PATH);
            } else {
                sw = File.CreateText(LOG_FILE_PATH);
            }
            sw.WriteLine("[ " + type + " ] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + log);
            sw.Close();
        }

        public static void WriteLog() {
            lock (log_list) {
                while (log_list.Count > 0) {
                    LogItem li = (LogItem)log_list.Dequeue();
                    AddLog(li.type, li.log);
                }
            }
        }

        public static void LOG(string type, string log) {
            lock (log_list) {
                log_list.Enqueue(new LogItem(type, log));
            }
        }

        public static void Init() {
            log_list = new Queue();
            lock (log_list) {
                log_list.Clear();
            }
        }
    }

    public class LogItem {
        public string type;
        public string log;
        public LogItem() {
            type = "undef";
            log = "";
        }
        public LogItem(string _type, string _log) {
            type = _type;
            log = _log;
        }
    }

    public class Action {
        public const int ACTION = 2;
        public const int POSTURE = 1;
        public const int UNDEF = 0;

        public int uid;
        public string name;
        public int type;

        public Action() {
            uid = 0;
            name = "";
            type = UNDEF;
        }

        public Action(int _uid, int _type, string _name = "") {
            uid = _uid;
            type = _type;
            name = _name;
        }
    }
}