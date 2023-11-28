using log4net;
using log4net.Config;
using ReactiveDemo.Helpers.Log;
using ReactiveDemo.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Util
{
    public class LogUtil
    {
        #region Constants

        private const string DEFAULT_LOG4NETCONFIG_FILENAME = "log4net.config";

        #endregion

        #region singleton

        private static LogUtil _instance = null;

        static LogUtil()
        {
        }

        #endregion

        #region Static Properties

        /// <summary>
        ///   Get an instance of the log4net library's logging context.
        /// </summary>
        public static ILog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogUtil();
                }

                return _instance.Logger;
            }
        }

        /// <summary>
        ///   Global context mapping definition containing data originating from currently running processes
        /// </summary>
        public static Dictionary<string, string> CurrentProcessGlobalContextConfig = new Dictionary<string, string>()
        {
            { "ProcPriorityBoostEnabled",       "PriorityBoostEnabled"       },
            { "ProcPeakVirtualMemorySize64",    "PeakVirtualMemorySize64"    },
            { "ProcPeakVirtualMemorySize",      "PeakVirtualMemorySize"      },
            { "ProcPeakWorkingSet64",           "PeakWorkingSet64"           },
            { "ProcPeakWorkingSet",             "PeakWorkingSet"             },
            { "ProcPeakPagedMemorySize64",      "PeakPagedMemorySize64"      },
            { "ProcPrivateMemorySize",          "PrivateMemorySize"          },
            { "ProcPeakPagedMemorySize",        "PeakPagedMemorySize"        },
            { "ProcPagedSystemMemorySize",      "PagedSystemMemorySize"      },
            { "ProcPagedMemorySize64",          "PagedMemorySize64"          },
            { "ProcPagedMemorySize",            "PagedMemorySize"            },
            { "ProcNonpagedSystemMemorySize64", "NonpagedSystemMemorySize64" },
            { "ProcNonpagedSystemMemorySize",   "NonpagedSystemMemorySize"   },
            { "ProcMinWorkingSet",              "MinWorkingSet"              },
            { "ProcPagedSystemMemorySize64",    "PagedSystemMemorySize64"    },
            { "ProcPrivateMemorySize64",        "PrivateMemorySize64"        },
            { "ProcPrivilegedProcessorTime",    "PrivilegedProcessorTime"    },
            { "ProcProcessName",                "ProcessName"                },
            { "ProcWorkingSet64",               "WorkingSet64"               },
            { "ProcWorkingSet",                 "WorkingSet"                 },
            { "ProcEnableRaisingEvents",        "EnableRaisingEvents"        },
            { "ProcVirtualMemorySize64",        "VirtualMemorySize64"        },
            { "ProcVirtualMemorySize",          "VirtualMemorySize"          },
            { "ProcUserProcessorTime",          "UserProcessorTime"          },
            { "ProcTotalProcessorTime",         "TotalProcessorTime"         },
            { "ProcStartTime",                  "StartTime"                  },
            { "ProcSessionId",                  "SessionId"                  },
            { "ProcResponding",                 "Responding"                 },
            { "ProcProcessorAffinity",          "ProcessorAffinity"          },
            { "ProcMaxWorkingSet",              "MaxWorkingSet"              },
            { "ProcMainWindowTitle",            "MainWindowTitle"            },
            { "ProcMachineName",                "MachineName"                },
            { "ProcId",                         "Id"                         },
            { "ProcHandleCount",                "HandleCount"                },
            { "ProcHandle",                     "Handle"                     },
            { "ProcExitTime",                   "ExitTime"                   },
            { "ProcHasExited",                  "HasExited"                  },
            { "ProcExitCode",                   "ExitCode"                   },
            { "ProcBasePriority",               "BasePriority"               },
            { "ProcMainWindowHandle",           "MainWindowHandle"           },
        };

        /// <summary>
        ///   A mapping definition for the global context containing data originating from the GUI resources of the currently running process
        /// </summary>
        public static Dictionary<string, uint> GuiResourcesGlobalContextConfig = new Dictionary<string, uint>()
        {
            { "GuiGdiObjects",  GuiResourcesHelper.GR_GDIOBJECTS  },
            { "GuiUserObjects", GuiResourcesHelper.GR_USEROBJECTS },
        };

        #endregion

        #region Static Methods

        /// <summary>
        ///   Sets the specified log4net library's logging context instance as the current logging context.
        /// </summary>
        /// <param name="logger">
        ///   Specifies a logging context instance for the log4net library.
        /// </param>
        public static void SetLogger(ILog logger)
        {
            _instance = new LogUtil(logger);
        }

        /// <summary>
        ///   Sets data originating from the currently running process into the global context.
        /// </summary>
        public static void SetCurrentProcessGlobalContext()
        {
            if (CurrentProcessGlobalContextConfig != null && CurrentProcessGlobalContextConfig.Count > 0)
            {
                foreach (var kvpair in CurrentProcessGlobalContextConfig)
                {
                    try
                    {
                        GlobalContext.Properties[kvpair.Key] = new CurrentProcessHelper(kvpair.Value);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///   Sets data resulting from the currently running process's GUI resources into the global context.
        /// </summary>
        public static void SetGuiResourcesGlobalContext()
        {
            if (GuiResourcesGlobalContextConfig != null && GuiResourcesGlobalContextConfig.Count > 0)
            {
                foreach (var kvpair in GuiResourcesGlobalContextConfig)
                {
                    try
                    {
                        GlobalContext.Properties[kvpair.Key] = new GuiResourcesHelper(kvpair.Value);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///   Unsets data originating from the currently running process in the global context.
        /// </summary>
        public static void UnsetCurrentProcessGlobalContext()
        {
            if (CurrentProcessGlobalContextConfig != null && CurrentProcessGlobalContextConfig.Count > 0)
            {
                foreach (var kvpair in CurrentProcessGlobalContextConfig)
                {
                    try
                    {
                        GlobalContext.Properties.Remove(kvpair.Key);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///   Data resulting from the GUI resources of the currently running process is not set in the global context.
        /// </summary>
        public static void UnsetGuiResourcesGlobalContext()
        {
            if (GuiResourcesGlobalContextConfig != null && GuiResourcesGlobalContextConfig.Count > 0)
            {
                foreach (var kvpair in GuiResourcesGlobalContextConfig)
                {
                    try
                    {
                        GlobalContext.Properties.Remove(kvpair.Key);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///   Outputs the specified exception to the log.
        /// </summary>
        /// <param name="ex">
        ///   Specify the exception to be output to the log.
        /// </param>
        public static void LogException(Exception ex)
        {
            Instance.Error(ex.Message);
            Instance.Debug(ex.Message, ex);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Logging context instance of log4net library
        /// </summary>
        public ILog Logger { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        ///   constructor
        /// </summary>
        private LogUtil(ILog logger = null)
        {
            if (logger == null)
            {
                LoadDefaultConfiguration();
            }
            else
            {
                Logger = logger;
            }
        }

        #endregion

        #region Private Methods

        private void LoadDefaultConfiguration()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("LogUtil START Configuration"));

            Assembly a = Assembly.GetExecutingAssembly();

            sb.AppendLine(string.Format("Assembly: {0}", a.GetName()));

            var fileLogConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_LOG4NETCONFIG_FILENAME);
            var logConfig = new FileInfo(fileLogConfigPath);

            sb.AppendLine(string.Format("Log Config: {0}", logConfig));

            if (!logConfig.Exists)
            {
                sb.AppendLine(string.Format("Failed to load logging configuration."));
            }
            else
            {
                var hierarchy = CustomerLogManager.Instance.CreateLogger(fileLogConfigPath);
                Logger = LogManager.GetLogger(hierarchy.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            sb.AppendLine(string.Format("START Adding CurrentProcessHelper Properties to GlobalContext."));
            SetCurrentProcessGlobalContext();
            sb.AppendLine(string.Format("END Adding CurrentProcessHelper Properties to GlobalContext."));

            sb.AppendLine(string.Format("START Adding GuiResourcesHelper Properties to GlobalContext."));
            SetGuiResourcesGlobalContext();
            sb.AppendLine(string.Format("END Adding GuiResourcesHelper Properties to GlobalContext."));

            sb.AppendLine(string.Format("START Adding Custom Properties to GlobalContext."));
            GlobalContext.Properties["RequestID"] = "";
            sb.AppendLine(string.Format("END Adding Custom Properties to GlobalContext."));

            sb.AppendLine(string.Format("LogUtil END Configuration"));

            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_LOG4NETCONFIG_FILENAME + ".log"), sb.ToString(), Encoding.UTF8);
        }

        #endregion
    }
}
