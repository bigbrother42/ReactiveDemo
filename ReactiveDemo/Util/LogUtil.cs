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
        #region 定数

        private const string DEFAULT_LOG4NETCONFIG_FILENAME = "log4net.config";

        #endregion

        #region シングルトン

        private static LogUtil _instance = null;

        static LogUtil()
        {
        }

        #endregion

        #region Static Properties

        /// <summary>
        ///   log4netライブラリのロギングコンテキストのインスタンスを取得します。
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
        ///   現在実行中のプロセスに起因するデータを含むグローバルコンテキストのマッピング定義
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
        ///   現在実行中のプロセスのGUIリソースに起因するデータを含むグローバルコンテキストのマッピング定義
        /// </summary>
        public static Dictionary<string, uint> GuiResourcesGlobalContextConfig = new Dictionary<string, uint>()
        {
            { "GuiGdiObjects",  GuiResourcesHelper.GR_GDIOBJECTS  },
            { "GuiUserObjects", GuiResourcesHelper.GR_USEROBJECTS },
        };

        #endregion

        #region Static Methods

        /// <summary>
        ///   指定されたlog4netライブラリのロギングコンテキストインスタンスを現在のロギングコンテキストとして設定します。
        /// </summary>
        /// <param name="logger">
        ///   log4netライブラリのロギングコンテキストインスタンスを指定します。
        /// </param>
        public static void SetLogger(ILog logger)
        {
            _instance = new LogUtil(logger);
        }

        /// <summary>
        ///   現在実行中のプロセスに起因するデータをグローバルコンテキストに設定します。
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
        ///   現在実行中のプロセスのGUIリソースに起因するデータをグローバルコンテキストに設定します。
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
        ///   現在実行中のプロセスに起因するデータをグローバルコンテキストに未設定します。
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
        ///   現在実行中のプロセスのGUIリソースに起因するデータをグローバルコンテキストに未設定します。
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
        ///   指定された例外をログへ出力します。
        /// </summary>
        /// <param name="ex">
        ///   ログへ出力する例外を指定します。
        /// </param>
        public static void LogException(Exception ex)
        {
            Instance.Error(ex.Message);
            Instance.Debug(ex.Message, ex);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   log4netライブラリのロギングコンテキストインスタンス
        /// </summary>
        public ILog Logger { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        ///   コンストラクタ
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
            var embeddedLogConfigPath = a.GetName().Name + "." + DEFAULT_LOG4NETCONFIG_FILENAME;

            var logConfig = new FileInfo(fileLogConfigPath);

            sb.AppendLine(string.Format("Log Config: {0}", logConfig));

            if (!logConfig.Exists)
            {
                var isEmbeddedConfigLoaded = LoadEmbeddedConfiguration(fileLogConfigPath, embeddedLogConfigPath);
                if (isEmbeddedConfigLoaded)
                {
                    using (Stream resFilestream = a.GetManifestResourceStream(embeddedLogConfigPath))
                    {
                        var hierarchy = CustomerLogManager.Instance.CreateLogger(resFilestream);
                        Logger = LogManager.GetLogger(hierarchy.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    }
                }
                else
                {
                    sb.AppendLine(string.Format("Failed to load logging configuration."));
                }
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

            sb.AppendLine(string.Format("EmLogUtil END Configuration"));

            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_LOG4NETCONFIG_FILENAME + ".log"), sb.ToString(), Encoding.UTF8);
        }

        private bool LoadEmbeddedConfiguration(string fileLogConfigPath, string embeddedLogConfigPath)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            bool isEmbeddedConfigLoaded = false;

            using (Stream resFilestream = a.GetManifestResourceStream(embeddedLogConfigPath))
            {
                if (resFilestream != null)
                {
                    try
                    {
                        BinaryReader br = new BinaryReader(resFilestream);
                        FileStream fs = new FileStream(fileLogConfigPath, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        byte[] ba = new byte[resFilestream.Length];
                        resFilestream.Read(ba, 0, ba.Length);
                        bw.Write(ba);
                        br.Close();
                        bw.Close();
                        resFilestream.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                        XmlConfigurator.Configure(resFilestream);
                    }

                    isEmbeddedConfigLoaded = true;
                }
            }
            return isEmbeddedConfigLoaded;
        }

        #endregion
    }
}
