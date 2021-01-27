using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FancyToys.Pages.Dialog;
using FancyToys.Pages.Nursery;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using FancyToys.Pages.Server;
using Windows.UI;

namespace FancyToys.Pages
{
    public enum LogType
    {
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Fatal = 6,
        Dialog = 7,     // 用于在前端的提示

    }
    public struct LoggingStruct
    {
        public LogType type;
        public string content;
    }

    class LoggingManager
    {
        private static Queue<LoggingStruct> logCache = new Queue<LoggingStruct>();
        private static readonly Dictionary<LogType, Color> logColor = new Dictionary<LogType, Color>()
        {
            { LogType.Trace, Colors.LightGray },
            { LogType.Debug, Colors.LightBlue },
            { LogType.Info, Colors.LightGreen },
            { LogType.Warn, Colors.Yellow },
            { LogType.Error, Colors.PaleVioletRed},
            { LogType.Fatal, Colors.DarkRed}
        };
        public static void Deal(string message)
        {
            LoggingStruct ls = JsonConvert.DeserializeObject<LoggingStruct>(message);

            switch (ls.type)
            {
                case LogType.Dialog:
                    Dialog(ls.content);
                    break;
                case LogType.Trace:
                    Trace(ls.content);
                    break;
                case LogType.Info:
                    Info(ls.content);
                    break;
                case LogType.Debug:
                    Debug(ls.content);
                    break;
                case LogType.Warn:
                    Warn(ls.content);
                    break;
                case LogType.Error:
                    Error(ls.content);
                    break;
                case LogType.Fatal:
                    Fatal(ls.content);
                    break;
            }
        }

        public static async void Dialog(string message)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
             {
                 await MessageDialog.Info(message, "");
             });
        }

        public static void Trace(string msg, int depth = 1)
        {
            PrintToPage(LogType.Trace, CallerName(depth + 1) + msg);
        }

        public static void Info(string msg, int depth = 1)
        {
            PrintToPage(LogType.Info, CallerName(depth + 1) + msg);
        }

        public static void Debug(string msg, int depth = 1)
        {
            PrintToPage(LogType.Debug, CallerName(depth + 1) + msg);
        }

        public static void Warn(string msg, int depth = 1)
        {
            PrintToPage(LogType.Warn, CallerName(depth + 1) + msg);
        }

        public static void Error(string msg, int depth = 1)
        {
            PrintToPage(LogType.Error, CallerName(depth + 1) + msg);
        }

        public static void Fatal(string msg, int depth = 1)
        {
            PrintToPage(LogType.Fatal, CallerName(depth + 1) + msg);
        }

        private static async void PrintToPage(LogType type, string message)
        {
            Color color = Colors.White;

            FancyServer page = FancyServer.GetThis();
            // System.NullReferenceException:“Object reference not set to an instance of an object.”
            // page: null
            if (page == null)
            {
                logCache.Enqueue(new LoggingStruct
                {
                    type = type,
                    content = message
                });
            }
            else
            {
                if (logCache.Count > 0)
                {
                    while (logCache.Count > 0)
                    {
                        LoggingStruct ls = logCache.Dequeue();
                        await CoreApplication.MainView.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal, () =>
                            {
                                page.UpdateLog(logColor[ls.type], ls.content);
                            });
                    }
                }
                await CoreApplication.MainView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal, () =>
                    {
                        page.UpdateLog(logColor[type], message);
                    });
            } 
        }

        private static string CallerName(int depth)
        {
            MethodBase method = new StackTrace().GetFrame(depth).GetMethod();
            return $"[{method.ReflectedType.Name}.{method.Name}] ";
        }
    }
}